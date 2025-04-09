using System;
using System.Linq;
using Std.Tools.Core;
using Std.Tools.Core.Definition;
using Std.Tools.Core.Transforms;


namespace Std.Tools.Projects.Transforms
{
	public sealed class UpgradeTestServiceTransformation : ITransformation
	{
		public void Transform(Project definition)
		{
			var removeQueue = definition.ItemGroups
				.ElementsAnyNamespace("Service")
				.Where(x => !string.IsNullOrEmpty(x.Value) && string.Equals(x.Value, "{82A7F48D-3B50-4B1E-B82E-3ADA8210C358}", StringComparison.OrdinalIgnoreCase))
				.ToArray();

			foreach (var element in removeQueue)
			{
				element.Remove();
			}
		}
	}
}
