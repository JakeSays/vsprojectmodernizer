using System.Collections.Generic;
using Std.Tools.Core.Definition;


namespace Std.Tools.Core.Analysis
{
	public interface IReporter<TOptions> where TOptions : IReporterOptions
	{
		/// <summary>
		/// Default options for any project
		/// </summary>
		TOptions DefaultOptions { get; }

		/// <summary>
		/// Do the actual issue reporting
		/// </summary>
		/// <param name="results">Diagnostics to report</param>
		/// <param name="reporterOptions">Options for the reporter</param>
		void Report(IReadOnlyList<IDiagnosticResult> results, TOptions reporterOptions);

		TOptions CreateOptionsForProject(Project project);
	}
}
