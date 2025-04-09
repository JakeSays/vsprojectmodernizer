using System;
using System.Collections.Immutable;
using System.Linq;
using Std.Tools.Core;
using Std.Tools.Core.Definition;
using Std.Tools.Core.Transforms;


namespace Std.Tools.Projects.Transforms
{
	public sealed class UpgradeDebugTypeTransformation : ITransformation
	{
		public void Transform(Project definition)
		{
			var removeQueue = definition.PropertyGroups
				.ElementsAnyNamespace("DebugType")
				.Where(x => !string.Equals(x.Value, "portable", StringComparison.OrdinalIgnoreCase))
				.ToImmutableArray();

			foreach (var element in removeQueue)
			{
				element.Remove();
			}
		}
	}
}
