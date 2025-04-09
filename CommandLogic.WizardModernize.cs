using System;
using System.Collections.Generic;
using System.IO;
using Serilog;
using Std.Tools.Core;
using Std.Tools.Core.Definition;
using Std.Tools.Core.Writing;


namespace Std.Tools
{
    public partial class CommandLogic
    {
        private void WizardModernize(IReadOnlyCollection<Project> projects,
            ITransformationSet transformationSet,
            ConversionOptions conversionOptions)
        {
            var transformations = transformationSet.CollectAndOrderTransformations(_facility.Logger, conversionOptions);

            var doBackups = AskBinaryChoice("Would you like to create backups?");

            var writer = new ProjectWriter(_facility.Logger, new ProjectWriteOptions { MakeBackups = doBackups });

            foreach (var project in projects)
            {
                using (_facility.Logger.BeginScope(project.FilePath))
                {
                    var projectName = Path.GetFileNameWithoutExtension(project.FilePath.Name);
                    Log.Information("Modernizing {ProjectName}...", projectName);

                    if (!project.Valid)
                    {
                        Log.Error("Project {ProjectName} is marked as invalid, skipping...", projectName);
                        continue;
                    }

                    foreach (var transformation in transformations.WhereSuitable(project, conversionOptions))
                    {
                        try
                        {
                            transformation.Transform(project);
                        }
                        catch (Exception e)
                        {
                            Log.Error(e, "Transformation {Item} has thrown an exception, skipping...",
                                transformation.GetType().Name);
                        }
                    }

                    if (!writer.TryWrite(project))
                    {
                        continue;
                    }

                    Log.Information("Project {ProjectName} has been modernized", projectName);
                }
            }
        }
    }
}
