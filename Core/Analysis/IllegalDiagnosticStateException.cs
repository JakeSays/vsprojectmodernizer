using System;


namespace Std.Tools.Core.Analysis
{
	public sealed class IllegalDiagnosticStateException : InvalidOperationException
	{
		/// <inheritdoc />
		public IllegalDiagnosticStateException()
		{
		}

		/// <inheritdoc />
		public IllegalDiagnosticStateException(string message) : base(message)
		{
		}

		/// <inheritdoc />
		public IllegalDiagnosticStateException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
