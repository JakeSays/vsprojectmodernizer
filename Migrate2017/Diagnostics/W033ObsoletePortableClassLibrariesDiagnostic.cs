using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Std.Tools.Core.Analysis;
using Std.Tools.Core.Definition;


namespace Std.Tools.Projects.Diagnostics
{
	public sealed class W033ObsoletePortableClassLibrariesDiagnostic : DiagnosticBase
	{
		public W033ObsoletePortableClassLibrariesDiagnostic() : base(33)
		{
		}

		public override IReadOnlyList<IDiagnosticResult> Analyze(Project project)
		{
			var pcls = project.TargetFrameworks
				.Where(x => x.StartsWith("portable-", StringComparison.OrdinalIgnoreCase))
				.ToImmutableHashSet();

			// not all profiles can be mapped to .NET Standard (thanks to Silverlight & Framework 4.0)
			// we could skip emitting diagnostics in such cases, but we don't
			// instead we suggest dropping such old targets (WinXP can still be covered by net40 in TargetFrameworks)

			var list = new List<IDiagnosticResult>(pcls.Count);

			foreach (var pcl in pcls)
			{
				list.Add(
					CreateDiagnosticResult(project,
						$"PCL profiles are obsolete. Consider migrating to .NET Standard.",
						project.FilePath));
			}

			return list;
		}
	}
}
