using Std.Tools.Core.Definition;


namespace Std.Tools.Core.Transforms
{
	public interface ITransformation
	{
		/// <summary>
		/// Alter the provided project in some way
		/// </summary>
		/// <param name="definition"></param>
		void Transform(Project definition);
	}
}
