using System;
using System.Collections.Generic;
using Std.Tools.Core.Analysis;
using Std.Tools.Core.Definition;


namespace Std.Tools.Projects.Diagnostics
{
	public sealed class W032OldLanguageVersionDiagnostic : DiagnosticBase
	{
		public W032OldLanguageVersionDiagnostic() : base(32)
		{
		}

		public override IReadOnlyList<IDiagnosticResult> Analyze(Project project)
		{
			var list = new List<IDiagnosticResult>();

			foreach (var x in project.ProjectDocument.Descendants(project.XmlNamespace + "LangVersion"))
			{
				// last 2 versions + default
				var version = x.Value;
				if (version.Equals("7.2", StringComparison.OrdinalIgnoreCase)) continue;
				if (version.Equals("7.3", StringComparison.OrdinalIgnoreCase)) continue;
				if (version.Equals("8.0", StringComparison.OrdinalIgnoreCase)) continue;
				if (version.Equals("latest", StringComparison.OrdinalIgnoreCase)) continue;
				if (version.Equals("default", StringComparison.OrdinalIgnoreCase)) continue;

				list.Add(
					CreateDiagnosticResult(project,
							$"Consider upgrading language version to the latest ({version}).",
							project.FilePath)
						.LoadLocationFromElement(x));
			}

			return list;
		}
	}
}
