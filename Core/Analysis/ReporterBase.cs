using System.Collections.Generic;
using Std.Tools.Core.Definition;


namespace Std.Tools.Core.Analysis
{
	public abstract class ReporterBase<TOptions> : IReporter<TOptions> where TOptions : IReporterOptions
	{
		public abstract TOptions DefaultOptions { get; }

		protected abstract void Report(IDiagnosticResult result, TOptions reporterOptions);

		/// <inheritdoc />
		public void Report(IReadOnlyList<IDiagnosticResult> results, TOptions reporterOptions)
		{
			if (results == null || results.Count == 0)
			{
				return;
			}

			foreach (var result in results)
			{
				var targetResult = result;
				if (result.Location?.Source != null)
				{
					var sourcePath = result.Project.TryFindBestRootDirectory()
						?.GetRelativePathTo(result.Location.Source);
					targetResult = new DiagnosticResult(result)
					{
						Location = new DiagnosticLocation(result.Location)
						{
							SourcePath = sourcePath
						}
					};
				}

				Report(targetResult, reporterOptions);
			}
		}

		public virtual TOptions CreateOptionsForProject(Project project)
		{
			return this.DefaultOptions;
		}
	}
}
