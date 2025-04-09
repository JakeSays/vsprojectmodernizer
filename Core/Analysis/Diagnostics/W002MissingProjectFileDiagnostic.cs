using System;
using System.Collections.Generic;
using Std.Tools.Core.Definition;


namespace Std.Tools.Core.Analysis.Diagnostics
{
	public sealed class W002MissingProjectFileDiagnostic : DiagnosticBase
	{
		public override bool SkipForLegacyProject => true;
		public override bool SkipForModernProject => true;
		public override IReadOnlyList<IDiagnosticResult> Analyze(Project project) =>
			throw new InvalidOperationException("W002 is not an executable diagnostic");

		public static IReadOnlyList<IDiagnosticResult> CreateResult(ProjectReference @ref, Solution solution = null)
		{
			return new[]
			{
				new DiagnosticResult
				{
					Code = "W002",
					Message =
						$"Referenced project file '{@ref.Include}' was not found at '{@ref.ProjectFile.FullName}'.",
					Location = new DiagnosticLocation
					{
						Source = solution?.FilePath
					}
				}
			};
		}

		public W002MissingProjectFileDiagnostic() : base(2)
		{
		}
	}
}
