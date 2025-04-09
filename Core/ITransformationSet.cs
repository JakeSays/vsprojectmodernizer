using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Std.Tools.Core.Transforms;


namespace Std.Tools.Core
{
	public interface ITransformationSet
	{
		IReadOnlyCollection<ITransformation> Transformations(
			ILogger logger,
			ConversionOptions conversionOptions);
	}
}
