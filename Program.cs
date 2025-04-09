using System;
using Serilog;
using Serilog.Events;
using Std.Tools.Core;
using Std.Tools.Core.Analysis;
using Std.Tools.Core.Caching;
using Std.Tools.Core.Transforms;
using Std.Tools.Microsoft.DotNet.Cli.CommandLine;
using Std.Tools.Migrate2019;
using static Std.Tools.Microsoft.DotNet.Cli.CommandLine.Accept;
using static Std.Tools.Microsoft.DotNet.Cli.CommandLine.Create;

namespace Std.Tools
{
	internal static class Program
	{
		private static int Main(string[] args)
		{
			ProgramBase.CreateLogger();

			try
			{
				var argParser = new Parser(options: Command("migrator",
					".NET Project Migration Tool",
					NoArguments(),
					ProgramBase.Wizard(),
					ProgramBase.Evaluate(),
					ProgramBase.Migrate(),
					ProgramBase.Analyze(),
					ProgramBase.HelpOption(),
					ProgramBase.VerbosityOption()));

				var result = argParser.Parse(args);
				return ProcessArgs(result);
			}
			catch (HelpException e)
			{
				Log.Information(e.Message);
				return 0;
			}
			catch (Exception e)
			{
				if (Log.IsEnabled(LogEventLevel.Debug))
					Log.Fatal(e, "Fatal exception occurred");
				else
					Log.Fatal(e.Message);
				if (e is CommandParsingException commandParsingException)
				{
					Log.Information(commandParsingException.HelpText);
				}

				return 1;
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		private static int ProcessArgs(ParseResult result)
		{
			result.ShowHelpOrErrorIfAppropriate();

			var command = result.AppliedCommand();
			var globalOptions = result["migrator"];
			ProgramBase.ApplyVerbosity(result, globalOptions);

			Log.Verbose(result.Diagram());

			var items = command.Value<string[]>();

			var conversionOptions = new ConversionOptions
			{
				ProjectCache = new DefaultProjectCache(),
				ForceOnUnsupportedProjects = command.ValueOrDefault<bool>("force"),
				KeepAssemblyInfo = command.ValueOrDefault<bool>("keep-assembly-info")
			};

			switch (command.Name)
			{
				case "evaluate":
				case "migrate":
					var frameworks = command.ValueOrDefault<string[]>("target-frameworks");
					if (frameworks != null)
						conversionOptions.TargetFrameworks = frameworks;
					break;
			}

			var logic = new CommandLogic();
			switch (command.Name)
			{
				case "wizard":
					var diagnostics = new DiagnosticSet(Vs16DiagnosticSet.All);
					diagnostics.ExceptWith(DiagnosticSet.All);
					var sets = new WizardTransformationSets
					{
						MigrateSet = new ChainTransformationSet(
							new BasicSimplifyTransformationSet(Vs16TransformationSet.TargetVisualStudioVersion),
							Vs16TransformationSet.TrueInstance),
						ModernCleanUpSet = new BasicSimplifyTransformationSet(
							Vs16TransformationSet.TargetVisualStudioVersion),
						ModernizeSet = new ChainTransformationSet(
							new BasicSimplifyTransformationSet(Vs16TransformationSet.TargetVisualStudioVersion),
							Vs16ModernizationTransformationSet.TrueInstance),
						Diagnostics = diagnostics
					};

					logic.ExecuteWizard(items, conversionOptions, sets);
					break;
				case "evaluate":
					logic.ExecuteEvaluate(items, conversionOptions, Vs16TransformationSet.Instance, new AnalysisOptions(Vs16DiagnosticSet.All));
					break;
				case "analyze":
					logic.ExecuteAnalyze(items, conversionOptions, new AnalysisOptions(Vs16DiagnosticSet.All));
					break;
				case "migrate":
					conversionOptions.AppendTargetFrameworkToOutputPath = !command.ValueOrDefault<bool>("old-output-path");

					var forceTransformations = command.ValueOrDefault<string[]>("force-transformations");
					if (forceTransformations != null)
						conversionOptions.ForceDefaultTransforms = forceTransformations;

					logic.ExecuteMigrate(items, command.ValueOrDefault<bool>("no-backup"), conversionOptions, Vs16TransformationSet.Instance);
					break;
			}

			return result.Execute().Code;
		}
	}
}
