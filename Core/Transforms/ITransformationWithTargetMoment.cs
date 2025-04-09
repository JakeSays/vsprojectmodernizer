namespace Std.Tools.Core.Transforms
{
	public interface ITransformationWithTargetMoment : ITransformation
	{
		TargetTransformationExecutionMoment ExecutionMoment { get; }
	}
}
