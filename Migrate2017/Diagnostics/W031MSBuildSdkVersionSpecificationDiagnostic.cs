using System;
using System.Collections.Generic;
using Std.Tools.Core.Analysis;
using Std.Tools.Core.Definition;


namespace Std.Tools.Projects.Diagnostics
{
	public sealed class W031MSBuildSdkVersionSpecificationDiagnostic : DiagnosticBase
	{
		public W031MSBuildSdkVersionSpecificationDiagnostic() : base(31)
		{
		}

		public override IReadOnlyList<IDiagnosticResult> Analyze(Project project)
		{
			var root = project.ProjectDocument.Root ?? throw new ArgumentNullException(nameof(project));
			var sdk = root.Attribute("Sdk")?.Value?.Trim();

			if (string.IsNullOrEmpty(sdk))
			{
				return Array.Empty<IDiagnosticResult>();
			}

			if (!sdk.Contains("/"))
			{
				return Array.Empty<IDiagnosticResult>();
			}

			return new[]
			{
				CreateDiagnosticResult(project,
						$"You have MSBuild SDK version specified in your project file ({sdk}). This is considered a bad practice. A recommended approach would be using global.json file for centralized SDK version management.",
						project.FilePath)
					.LoadLocationFromElement(root)
			};
		}
	}
}
