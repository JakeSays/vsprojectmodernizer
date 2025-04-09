using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Std.Tools.Core;
using Std.Tools.Core.Transforms;
using Std.Tools.Projects.Transforms;


namespace Std.Tools.Projects
{
	public sealed class Vs15ModernizationTransformationSet : ITransformationSet
	{
		public static readonly Vs15ModernizationTransformationSet TrueInstance =
			new Vs15ModernizationTransformationSet();

		public static readonly ITransformationSet Instance = new ChainTransformationSet(
			BasicReadTransformationSet.Instance,
			new BasicSimplifyTransformationSet(Vs15TransformationSet.TargetVisualStudioVersion),
			TrueInstance);

		private Vs15ModernizationTransformationSet()
		{
		}

		public IReadOnlyCollection<ITransformation> Transformations(
			ILogger logger,
			ConversionOptions conversionOptions)
		{
			return new ITransformation[]
			{
				new UpgradeDebugTypeTransformation(),
				new UpgradeUseDefaultOutputPathTransformation(),
				new UpgradeUseComVisibleDefaultTransformation(),
				new UpgradeTestServiceTransformation(),
				new UpgradeFrameworkAssembliesToNuGetTransformation(logger),
			};
		}
	}
}
