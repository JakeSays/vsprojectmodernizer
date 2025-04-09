using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Std.Tools.Core.Transforms;


namespace Std.Tools.Core
{
	public class NoopTransformationSet : ITransformationSet
	{
		public static readonly NoopTransformationSet Instance = new NoopTransformationSet();

		private NoopTransformationSet()
		{
		}

		public IReadOnlyCollection<ITransformation> Transformations(
			ILogger logger,
			ConversionOptions conversionOptions)
		{
			return Array.Empty<ITransformation>();
		}
	}
}
