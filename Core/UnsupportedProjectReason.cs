namespace Std.Tools.Core
{
	public enum UnsupportedProjectReason
	{
		/// <summary>
		/// Supported.
		/// </summary>
		Supported = 0,

		/// <summary>
		/// Not supported because entity framework is likely used.
		/// </summary>
		EntityFramework,

		/// <summary>
		/// Not supported because the project type is not supported, <seealso cref="https://github.com/dotnet/project-system/blob/master/docs/feature-comparison.md"/>.
		/// </summary>
		NotSupportedProjectType
	}
}
