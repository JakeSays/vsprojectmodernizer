using System.Collections.Generic;
using System.Linq;
using Std.Tools.Core;
using Std.Tools.Core.Definition;
using Std.Tools.Core.Transforms;


namespace Std.Tools.Projects.Transforms
{
	public sealed class AssemblyFilterPackageReferencesTransformation : ILegacyOnlyProjectTransformation
	{
		public void Transform(Project definition)
		{
			var packageReferences =
				definition.PackageReferences ?? new List<PackageReference>();

			var packageIds = packageReferences
				.Select(x => x.Id)
				.ToList();

			var (assemblyReferences, removeQueue) = definition.AssemblyReferences
				// We don't need to keep any references to package files as these are
				// now generated dynamically at build time
				.Split(assemblyReference => !packageIds.Contains(assemblyReference.Include));

			foreach (var assemblyReference in removeQueue)
			{
				assemblyReference.DefinitionElement?.Remove();
			}

			definition.AssemblyReferences = assemblyReferences;
		}
	}
}
