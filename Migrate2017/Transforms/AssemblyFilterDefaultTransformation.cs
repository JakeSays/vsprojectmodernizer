using System.Collections.Immutable;
using System.Linq;
using Std.Tools.Core;
using Std.Tools.Core.Definition;
using Std.Tools.Core.Transforms;


namespace Std.Tools.Projects.Transforms
{
	public sealed class AssemblyFilterDefaultTransformation : ILegacyOnlyProjectTransformation
	{
		public void Transform(Project definition)
		{
			if (definition.AssemblyReferences == null)
			{
				definition.AssemblyReferences = ImmutableArray<AssemblyReference>.Empty;
				return;
			}

			var (assemblyReferences, removeQueue) = definition.AssemblyReferences
				.Split(IsNonDefaultIncludedAssemblyReference);

			foreach (var assemblyReference in removeQueue)
			{
				assemblyReference.DefinitionElement?.Remove();
			}

			definition.AssemblyReferences = assemblyReferences;
		}


		private static bool IsNonDefaultIncludedAssemblyReference(AssemblyReference assemblyReference)
		{
			var name = assemblyReference.Include;
			return !new[]
			{
				"System",
				"System.Core",
				"System.Data",
				"System.Drawing",
				"System.IO.Compression.FileSystem",
				"System.Numerics",
				"System.Runtime.Serialization",
				"System.Xml",
				"System.Xml.Linq"
			}.Contains(name);
		}
	}
}
