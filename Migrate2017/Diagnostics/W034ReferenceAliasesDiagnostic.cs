using System.Collections.Generic;
using System.Linq;
using Std.Tools.Core.Analysis;
using Std.Tools.Core.Definition;


namespace Std.Tools.Projects.Diagnostics
{
	public sealed class W034ReferenceAliasesDiagnostic : DiagnosticBase
	{
		public W034ReferenceAliasesDiagnostic() : base(34)
		{
		}

		public override IReadOnlyList<IDiagnosticResult> Analyze(Project project)
		{
			var list = new List<IDiagnosticResult>();

			foreach (var reference in project.ProjectReferences.Where(x => !string.IsNullOrEmpty(x.Aliases)))
			{
				list.Add(
					CreateDiagnosticResult(project,
							$"ProjectReference ['{reference.Include}'] aliases are a feature of low support. Consider dropping their usage.",
							project.FilePath)
						.LoadLocationFromElement(reference.DefinitionElement));
			}

			return list;
		}
	}
}
