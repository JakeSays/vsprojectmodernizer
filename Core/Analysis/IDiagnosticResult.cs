using Std.Tools.Core.Definition;


namespace Std.Tools.Core.Analysis
{
	public interface IDiagnosticResult
	{
		string Code { get; }
		IDiagnosticLocation Location { get; }
		string Message { get; }
		Project Project { get; }
	}
}
