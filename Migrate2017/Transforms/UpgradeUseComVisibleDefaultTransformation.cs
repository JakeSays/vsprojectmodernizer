using System.Collections.Immutable;
using System.Linq;
using Std.Tools.Core;
using Std.Tools.Core.Definition;
using Std.Tools.Core.Transforms;


namespace Std.Tools.Projects.Transforms
{
	public sealed class UpgradeUseComVisibleDefaultTransformation : ITransformation
	{
		public void Transform(Project definition)
		{
			var removeQueue = definition.PropertyGroups
				.ElementsAnyNamespace("ComVisible")
				.Where(x => !string.IsNullOrEmpty(x.Value))
				.ToImmutableArray();

			foreach (var element in removeQueue)
			{
				element.Remove();
			}
		}
	}
}
