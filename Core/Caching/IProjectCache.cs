namespace Std.Tools.Core.Caching
{
	public interface IProjectCache
	{
		void Add(string key, Definition.Project project);

		bool TryGetValue(string key, out Definition.Project project);

		void Purge();
	}
}
