using System.Xml.Linq;


namespace Std.Tools.Core.Definition
{
	public interface IReference
	{
		XElement DefinitionElement { get; }
	}
}
