using Std.Tools.Core.Definition;


namespace Std.Tools.Core.Analysis
{
	public sealed class DiagnosticResult : IDiagnosticResult
	{
		public string Code { get; internal set; }
		public string Message { get; internal set; }
		public Project Project { get; internal set; }
		public IDiagnosticLocation Location { get; internal set; }

		public DiagnosticResult()
		{
		}

		public DiagnosticResult(IDiagnosticResult result)
		{
			this.Code = result.Code;
			this.Message = result.Message;
			this.Project = result.Project;
			this.Location = result.Location;
		}
	}
}
