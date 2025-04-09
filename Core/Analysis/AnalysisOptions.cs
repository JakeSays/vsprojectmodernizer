using System.Collections.Generic;
using System.Collections.Immutable;


namespace Std.Tools.Core.Analysis
{
	public sealed class AnalysisOptions
	{
		/// <summary>
		/// Including ID of diagnostics in this list will make analyzer skip their execution and therefore output
		/// </summary>
		public ImmutableHashSet<IDiagnostic> Diagnostics { get; }

		public AnalysisOptions(IEnumerable<IDiagnostic> diagnostics = null)
		{
			this.Diagnostics = (diagnostics ?? DiagnosticSet.All).ToImmutableHashSet();
		}
	}
}
