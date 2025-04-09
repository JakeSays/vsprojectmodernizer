using Std.Tools.Core.Analysis;
using Std.Tools.Projects;


namespace Std.Tools.Migrate2019
{
	public static class Vs16DiagnosticSet
	{
		public static readonly DiagnosticSet All = new DiagnosticSet();

		static Vs16DiagnosticSet()
		{
			All.UnionWith(Vs15DiagnosticSet.All);
		}
	}
}
