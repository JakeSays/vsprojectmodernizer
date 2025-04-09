using System;
using System.Collections.Immutable;
using System.Linq;
using Std.Tools.Core;
using Std.Tools.Core.Definition;
using Std.Tools.Core.Transforms;


namespace Std.Tools.Projects.Transforms
{
	public sealed class UpgradeUseDefaultOutputPathTransformation : ITransformation
	{
		public void Transform(Project definition)
		{
			var docFileQueue = definition.PropertyGroups
				.ElementsAnyNamespace("DocumentationFile")
				.Where(x => HasDefaultLegacyOutputPath(x.Value));

			var removeQueue = definition.PropertyGroups
				.ElementsAnyNamespace("OutputPath")
				.Where(x => IsDefaultLegacyOutputPath(x.Value))
				.Union(docFileQueue)
				.ToImmutableArray();

			foreach (var element in removeQueue)
			{
				element.Remove();
			}

			bool IsDefaultLegacyOutputPath(string x) =>
				string.Equals(
					x.Replace('\\', '/'),
					@"bin/$(Configuration)/", StringComparison.OrdinalIgnoreCase);

			bool HasDefaultLegacyOutputPath(string x) =>
				x
					.Replace('\\', '/')
					.StartsWith(@"bin/$(Configuration)/", StringComparison.OrdinalIgnoreCase);
		}
	}
}
