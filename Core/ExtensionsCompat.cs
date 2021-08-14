using System.IO;

namespace Project2015To2017
{
	public static partial class Extensions
	{
		public static string GetRelativePathTo(this FileSystemInfo from, FileSystemInfo to)
		{
			return Path.GetRelativePath(from.FullName, to.FullName);
		}
	}
}
