using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Std.Tools.Core.Transforms;


namespace Std.Tools.Core
{
	public class ChainTransformationSet : ITransformationSet
	{
		private readonly IReadOnlyCollection<ITransformationSet> sets;

		public ChainTransformationSet(IReadOnlyCollection<ITransformationSet> sets)
		{
			this.sets = sets ?? throw new ArgumentNullException(nameof(sets));
		}

		public ChainTransformationSet(params ITransformationSet[] sets)
			: this((IReadOnlyCollection<ITransformationSet>)sets)
		{
		}

		public IReadOnlyCollection<ITransformation> Transformations(ILogger logger, ConversionOptions conversionOptions)
		{
			var res = new List<ITransformation>();
			foreach (var set in sets)
			{
				res.AddRange(set.Transformations(logger, conversionOptions));
			}

			return res;
		}
	}
}
