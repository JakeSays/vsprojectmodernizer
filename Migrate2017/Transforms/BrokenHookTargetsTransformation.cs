using System.Collections.Immutable;
using System.Linq;
using Microsoft.Extensions.Logging;
using Std.Tools.Core;
using Std.Tools.Core.Definition;
using Std.Tools.Core.Transforms;


namespace Std.Tools.Projects.Transforms
{
	public sealed class BrokenHookTargetsTransformation : ILegacyOnlyProjectTransformation
	{
		private readonly ILogger logger;

		public BrokenHookTargetsTransformation(ILogger logger = null)
		{
			this.logger = logger ?? NoopLogger.Instance;
		}

		public void Transform(Project definition)
		{
			var beforeBuildArray = definition.Targets.Where(x => x.Attribute("Name")?.Value == "BeforeBuild")
				.ToImmutableArray();
			if (!beforeBuildArray.IsDefaultOrEmpty)
			{
				if (beforeBuildArray.Length > 1)
				{
					this.logger.LogWarning("Unexpected multiple BeforeBuild targets.");
				}

				var beforeBuild = beforeBuildArray.LastOrDefault();

				beforeBuild.SetAttributeValue("Name", "BeforeBuildMigrated");
				beforeBuild.SetAttributeValue("BeforeTargets", "PreBuildEvent");
			}

			var afterBuildArray = definition.Targets.Where(x => x.Attribute("Name")?.Value == "AfterBuild")
				.ToImmutableArray();
			if (!afterBuildArray.IsDefaultOrEmpty)
			{
				if (afterBuildArray.Length > 1)
				{
					this.logger.LogWarning("Unexpected multiple AfterBuild targets.");
				}

				var afterBuild = afterBuildArray.LastOrDefault();

				afterBuild.SetAttributeValue("Name", "AfterBuildMigrated");

				definition.SetProperty("BuildDependsOn", "$(BuildDependsOn);AfterBuildMigrated");
			}
		}
	}
}
