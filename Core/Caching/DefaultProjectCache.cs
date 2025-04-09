using System.Collections.Concurrent;
using Std.Tools.Core.Definition;


namespace Std.Tools.Core.Caching
{
	public sealed class DefaultProjectCache : IProjectCache
	{
		private readonly ConcurrentDictionary<string, Project> dictionary = new ConcurrentDictionary<string, Project>();

		public void Add(string key, Project project)
		{
			this.dictionary.AddOrUpdate(key, project, (s, p) => p);
		}

		public void Purge()
		{
			this.dictionary.Clear();
		}

		public bool TryGetValue(string key, out Project project)
		{
			return this.dictionary.TryGetValue(key, out project);
		}
	}
}
