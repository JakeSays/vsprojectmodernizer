using System.IO;


namespace Std.Tools.Core.Analysis
{
	public interface IDiagnosticLocation
	{
		FileSystemInfo Source { get; }
		uint SourceLine { get; }
		string SourcePath { get; }
	}
}
