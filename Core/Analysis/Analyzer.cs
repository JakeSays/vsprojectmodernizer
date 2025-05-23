using System;
using Std.Tools.Core.Analysis.Diagnostics;
using Std.Tools.Core.Definition;
using Std.Tools.Core.Reading;


namespace Std.Tools.Core.Analysis
{
	public sealed class Analyzer<TReporter, TReporterOptions>
		where TReporter : class, IReporter<TReporterOptions>
		where TReporterOptions : IReporterOptions
	{
		private readonly AnalysisOptions _options;
		private readonly TReporter _reporter;

		public Analyzer(TReporter reporter, AnalysisOptions options = null)
		{
			this._reporter = reporter ?? throw new ArgumentNullException(nameof(reporter));
			this._options = options ?? new AnalysisOptions();
		}

		public void Analyze(Project project)
		{
			if (project == null)
			{
				throw new ArgumentNullException(nameof(project));
			}

			foreach (var diagnostic in this._options.Diagnostics)
			{
				if (diagnostic.SkipForModernProject && project.IsModernProject)
				{
					continue;
				}

				if (diagnostic.SkipForLegacyProject && !project.IsModernProject)
				{
					continue;
				}

				var reporterOptions = this._reporter.CreateOptionsForProject(project);
				this._reporter.Report(diagnostic.Analyze(project), reporterOptions);
			}
		}

		public void Analyze(Solution solution)
		{
			if (solution == null)
			{
				throw new ArgumentNullException(nameof(solution));
			}

			if (solution.ProjectPaths == null)
			{
				return;
			}

			var projectReader = new ProjectReader(NoopLogger.Instance);
			foreach (var projectPath in solution.ProjectPaths)
			{
				if (!projectPath.ProjectFile.Exists)
				{
					if (this._options.Diagnostics.Contains(DiagnosticSet.W002))
					{
						this._reporter.Report(
							W002MissingProjectFileDiagnostic.CreateResult(projectPath, solution),
							this._reporter.DefaultOptions);
					}
					continue;
				}

				var project = projectReader.Read(projectPath.ProjectFile);

				Analyze(project);
			}
		}
	}
}
