using System.Collections.Generic;


namespace Std.Tools.Core.Transforms
{
	public interface ITransformationWithDependencies : ITransformation
	{
		IReadOnlyCollection<string> DependOn { get; }
	}
}
