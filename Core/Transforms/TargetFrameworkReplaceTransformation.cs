using System.Collections.Generic;
using Std.Tools.Core.Definition;


namespace Std.Tools.Core.Transforms
{
    public sealed class TargetFrameworkReplaceTransformation : ITransformationWithTargetMoment
    {
        public TargetFrameworkReplaceTransformation(IReadOnlyList<string> targetFrameworks,
            bool appendTargetFrameworkToOutputPath = true)
        {
            TargetFrameworks = targetFrameworks;
            AppendTargetFrameworkToOutputPath = appendTargetFrameworkToOutputPath;
        }

        public IReadOnlyList<string> TargetFrameworks { get; }
        public bool AppendTargetFrameworkToOutputPath { get; }

        public void Transform(Project definition)
        {
            if (definition == null)
            {
                return;
            }

            if (TargetFrameworks is { Count: > 0 })
            {
                definition.TargetFrameworks.Clear();

                foreach (var targetFramework in TargetFrameworks)
                {
                    definition.TargetFrameworks.Add(targetFramework);
                }
            }

            if (!AppendTargetFrameworkToOutputPath)
            {
                definition.AppendTargetFrameworkToOutputPath = false;
            }
        }

        public TargetTransformationExecutionMoment ExecutionMoment => TargetTransformationExecutionMoment.Early;
    }
}
