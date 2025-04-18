using System.Collections.Generic;
using System.IO;
using System.Linq;
using DotNet.Globbing;
using Serilog;
using Serilog.Extensions.Logging;
using Std.Tools.Core;
using Std.Tools.Core.Analysis;
using Std.Tools.Core.Transforms;
using Std.Tools.Core.Writing;


namespace Std.Tools
{
    public partial class CommandLogic
    {
        private readonly MigrationFacility _facility;

        private readonly PatternProcessor _globProcessor = (converter, pattern, callback, self) =>
        {
            Log.Verbose("Falling back to globbing");
            self.DoProcessableFileSearch();
            var glob = Glob.Parse(pattern);
            Log.Verbose("Parsed glob {Glob}", glob);

            foreach (var (path, extension) in self.Files)
            {
                if (!glob.IsMatch(path))
                {
                    continue;
                }

                var file = new FileInfo(path);
                callback(file, extension);
            }

            return true;
        };

        public CommandLogic()
        {
            var genericLogger = new SerilogLoggerProvider().CreateLogger(nameof(Serilog));
            _facility = new MigrationFacility(genericLogger, _globProcessor);
        }

        public void ExecuteEvaluate(IReadOnlyCollection<string> items,
            ConversionOptions conversionOptions,
            ITransformationSet transformationSet,
            AnalysisOptions analysisOptions) =>
            _facility.ExecuteEvaluate(items, conversionOptions, transformationSet, analysisOptions);

        public void ExecuteMigrate(IReadOnlyCollection<string> items,
            bool noBackup,
            ConversionOptions conversionOptions,
            ITransformationSet transformationSet)
        {
            var writeOptions = new ProjectWriteOptions { MakeBackups = !noBackup };
            _facility.ExecuteMigrate(items, transformationSet, conversionOptions, writeOptions);
        }

        public void ExecuteAnalyze(IReadOnlyCollection<string> items,
            ConversionOptions conversionOptions,
            AnalysisOptions analysisOptions) =>
            _facility.ExecuteAnalyze(items, conversionOptions, analysisOptions);

        public void ExecuteWizard(IReadOnlyCollection<string> items,
            ConversionOptions conversionOptions,
            WizardTransformationSets sets)
        {
            if (sets.MigrateSet == null ||
                sets.ModernCleanUpSet == null ||
                sets.ModernizeSet == null)
            {
                Log.Fatal("Wrong API usage: all transformation sets must be supplied");
                return;
            }

            conversionOptions.UnknownTargetFrameworkCallback = WizardUnknownTargetFrameworkCallback;

            var (projects, solutions) =
                _facility.ParseProjects(items, BasicReadTransformationSet.Instance, conversionOptions);

            if (projects.Count == 0)
            {
                Log.Information("No projects have been found to match your criteria.");
                return;
            }

            var (modern, legacy) = projects.Split(x => x.IsModernProject);

            foreach (var projectPath in modern.Select(x => x.FilePath))
            {
                Log.Information("Project {ProjectPath} is already CPS-based", projectPath);
            }

            foreach (var projectPath in solutions.SelectMany(x => x.UnsupportedProjectPaths))
            {
                Log.Warning("Project {ProjectPath} migration is not supported at the moment",
                    projectPath);
            }

            _facility.DoAnalysis(projects, new AnalysisOptions(DiagnosticSet.All));

            if (legacy.Count > 0)
            {
                WizardMigrate(legacy, sets.MigrateSet, conversionOptions);
            }
            else
            {
                Log.Information("It appears you already have everything converted to CPS.");

                if (AskBinaryChoice("Would you like to process CPS projects to clean up and reformat them?"))
                {
                    WizardModernCleanUp(modern, sets.ModernCleanUpSet, conversionOptions);
                }
            }

            conversionOptions.ProjectCache?.Purge();

            (projects, _) = _facility.ParseProjects(items, BasicReadTransformationSet.Instance, conversionOptions);

            Log.Information(
                "Modernization can be progressed a little further, but it might lead to unexpected behavioral changes.");

            if (AskBinaryChoice("Would you like to modernize projects?"))
            {
                WizardModernize(projects, sets.ModernizeSet, conversionOptions);

                conversionOptions.ProjectCache?.Purge();

                (projects, _) = _facility.ParseProjects(items, BasicReadTransformationSet.Instance, conversionOptions);
            }

            _facility.DoAnalysis(projects, new AnalysisOptions(sets.Diagnostics));
        }
    }
}
