using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Std.Tools.Core.Transforms;


namespace Std.Tools.Core
{
	public class BasicTransformationSet : ITransformationSet
	{
		private readonly IReadOnlyCollection<ITransformation> transformations;

		public BasicTransformationSet(IReadOnlyCollection<ITransformation> transformations)
		{
			this.transformations = transformations;
		}

		public BasicTransformationSet(params ITransformation[] transformations)
			: this((IReadOnlyCollection<ITransformation>)transformations)
		{
		}

		public IReadOnlyCollection<ITransformation> Transformations(ILogger logger, ConversionOptions conversionOptions)
		{
			return transformations;
		}
	}
}
