using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Std.Tools.Core;
using Std.Tools.Core.Transforms;
using Std.Tools.Projects;


namespace Std.Tools.Migrate2019
{
	public sealed class Vs16ModernizationTransformationSet : ITransformationSet
	{
		public static readonly Vs16ModernizationTransformationSet TrueInstance =
			new Vs16ModernizationTransformationSet();

		public static readonly ITransformationSet Instance = new ChainTransformationSet(
			BasicReadTransformationSet.Instance,
			new BasicSimplifyTransformationSet(Vs16TransformationSet.TargetVisualStudioVersion),
			Vs15ModernizationTransformationSet.TrueInstance,
			TrueInstance);

		private Vs16ModernizationTransformationSet()
		{
		}

		public IReadOnlyCollection<ITransformation> Transformations(
			ILogger logger,
			ConversionOptions conversionOptions)
		{
			return new ITransformation[]
			{
			};
		}
	}
}
