using System.Collections.Immutable;
using System.Linq;
using Microsoft.Extensions.Logging;
using Std.Tools.Core;
using Std.Tools.Core.Definition;
using Std.Tools.Core.Transforms;


namespace Std.Tools.Projects.Transforms
{
	public sealed class UpgradeFrameworkAssembliesToNuGetTransformation : IModernOnlyProjectTransformation
	{
		private readonly ILogger logger;

		public UpgradeFrameworkAssembliesToNuGetTransformation(ILogger logger = null)
		{
			this.logger = logger ?? NoopLogger.Instance;
		}

		public void Transform(Project definition)
		{
			var references = SystemNuGetPackages.DetectUpgradeableReferences(definition);
			foreach (var (_, _, assemblyReference) in references)
			{
				assemblyReference.DefinitionElement?.Remove();
			}

			definition.AssemblyReferences = definition.AssemblyReferences
				.Except(references.Select(x => x.reference))
				.ToImmutableArray();

			var packageReferences = references
				.Select(x => new PackageReference { Id = x.name, Version = x.version })
				.ToImmutableArray();

			var adjustedPackageReferences = definition.PackageReferences
				.Concat(packageReferences)
				.ToArray();

			foreach (var reference in packageReferences)
			{
				logger.LogDebug($"Adding NuGet reference to {reference.Id}, version {reference.Version}.");
			}

			definition.PackageReferences = adjustedPackageReferences;
		}
	}
}
