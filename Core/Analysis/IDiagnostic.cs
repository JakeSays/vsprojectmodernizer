using System.Collections.Generic;
using Std.Tools.Core.Definition;


namespace Std.Tools.Core.Analysis
{
	public interface IDiagnostic
	{
		bool SkipForLegacyProject { get; }
		bool SkipForModernProject { get; }

		IReadOnlyList<IDiagnosticResult> Analyze(Project project);
	}
}
