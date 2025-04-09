using Std.Tools.Core.Definition;


namespace Std.Tools.Core.Caching
{
	public sealed class NoProjectCache : IProjectCache
	{
		public static IProjectCache Instance => new NoProjectCache();

		private NoProjectCache()
		{

		}

		public void Add(string key, Project project)
		{
		}

		public bool TryGetValue(string key, out Project project)
		{
			project = null;
			return false;
		}

		public void Purge()
		{
		}
	}
}
