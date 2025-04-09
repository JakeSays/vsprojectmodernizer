using System.IO;
using System.Linq;
using Std.Tools.Core;
using Std.Tools.Core.Definition;
using Std.Tools.Core.Transforms;


namespace Std.Tools.Projects.Transforms
{
	public sealed class AssemblyFilterHintedPackageReferencesTransformation : ILegacyOnlyProjectTransformation
	{
		public void Transform(Project definition)
		{
			if (definition.PackageReferences == null || definition.PackageReferences.Count == 0)
			{
				return;
			}

			var projectPath = definition.ProjectFolder.FullName;

			var nugetRepositoryPath = definition.NuGetPackagesPath.FullName;

			var packageReferenceIds = definition.PackageReferences.Select(x => x.Id).ToArray();

			var packagePaths = packageReferenceIds.Select(packageId => Path.Combine(nugetRepositoryPath, packageId).ToLower())
				.ToArray();

			var (filteredAssemblies, removeQueue) = definition.AssemblyReferences
				.Split(assembly => !packagePaths.Any(
						packagePath => AssemblyMatchesPackage(assembly, packagePath)
					)
				);

			foreach (var assemblyReference in removeQueue)
			{
				assemblyReference.DefinitionElement?.Remove();
			}

			definition.AssemblyReferences = filteredAssemblies;

			bool AssemblyMatchesPackage(AssemblyReference assembly, string packagePath)
			{
				var hintPath = assembly.HintPath;
				if (hintPath == null)
				{
					return false;
				}

				hintPath = Extensions.MaybeAdjustFilePath(hintPath, projectPath);

				var fullHintPath = Path.IsPathRooted(hintPath) ? hintPath : Path.GetFullPath(Path.Combine(projectPath, hintPath));

				return fullHintPath.ToLowerInvariant().StartsWith(Extensions.MaybeAdjustFilePath(packagePath, projectPath));
			}
		}
	}
}
