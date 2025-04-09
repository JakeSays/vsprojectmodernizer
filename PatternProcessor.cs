using Std.Tools.Core;


namespace Std.Tools
{
	public delegate bool PatternProcessor(ProjectConverter converter, string pattern, ProcessSingleItemCallback callback, MigrationFacility self);
}
