using System.Collections.Generic;
using Std.Tools.Core;
using Std.Tools.Core.Analysis;


namespace Std.Tools
{
	public sealed class WizardTransformationSets
	{
		public ITransformationSet MigrateSet { get; set; }
		public ITransformationSet ModernCleanUpSet { get; set; }
		public ITransformationSet ModernizeSet { get; set; }
		public HashSet<IDiagnostic> Diagnostics { get; set; }
	}
}
