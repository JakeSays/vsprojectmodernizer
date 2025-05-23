using System.Collections.Generic;


namespace Std.Tools.Microsoft.DotNet.Cli.CommandLine
{
	public class OptionSet : OptionSet<Option>
	{
		protected override bool HasAlias(Option option, string alias) =>
			option.HasAlias(alias);

		protected override bool HasRawAlias(Option option, string alias) =>
			option.HasRawAlias(alias);

		protected override IReadOnlyCollection<string> RawAliasesFor(Option option) =>
			option.RawAliases;
	}
}
