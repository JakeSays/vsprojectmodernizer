using System;


namespace Std.Tools
{
	/// <summary>Allows control flow to be interrupted in order to display help in the console.</summary>
	internal sealed class HelpException : Exception
	{
		public HelpException(string message) : base(message)
		{
		}
	}

	internal sealed class CommandParsingException : Exception
	{
		public CommandParsingException(
			string message,
			string helpText = null) : base(message)
		{
			HelpText = helpText ?? "";
		}

		public string HelpText { get; }
	}
}
