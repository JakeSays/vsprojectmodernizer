using System.IO;


namespace Std.Tools.Core
{
	public static partial class Extensions
	{
		public static string GetRelativePathTo(this FileSystemInfo from, FileSystemInfo to)
		{
			return Path.GetRelativePath(from.FullName, to.FullName);
		}
	}
}
