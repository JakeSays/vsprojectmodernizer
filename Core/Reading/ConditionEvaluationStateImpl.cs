using System;
using System.Collections.Generic;
using Std.Tools.Core.Reading.Conditionals;


namespace Std.Tools.Core.Reading
{
	internal sealed class ConditionEvaluationStateImpl : IConditionEvaluationState
	{
		/// <inheritdoc />
		public Dictionary<string, List<string>> ConditionedPropertiesInProject { get; } =
			new Dictionary<string, List<string>>();

		public GenericExpressionNode Node { get; set; }

		public bool Evaluated { get; set; }

		public ICollection<OperatorExpressionNode> UnsupportedNodes { get; set; } =
			Array.Empty<OperatorExpressionNode>();

		public string Condition { get; }

		public ConditionEvaluationStateImpl(string condition)
		{
			this.Condition = condition ?? throw new ArgumentNullException(nameof(condition));
		}

		/// <inheritdoc />
		public string ExpandIntoStringBreakEarly(string expression)
		{
			return expression;
		}

		/// <inheritdoc />
		public string ExpandIntoString(string expression)
		{
			return expression;
		}

		public void Evaluate()
		{
			try
			{
				// it makes little sense for condition to be that short
				if (this.Condition.Length >= 2)
				{
					this.Node.Evaluate(this); // return value ignored
				}
			}
			catch (Exception)
			{
				// ignored
			}
			finally
			{
				this.Evaluated = true;
			}
		}
	}
}
