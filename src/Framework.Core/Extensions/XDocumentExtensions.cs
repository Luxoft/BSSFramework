using System.Xml;
using System.Xml.Linq;

using JetBrains.Annotations;

namespace Framework.Core;

public static class XDocumentExtensions
{
    public static XDocument OverrideChildrenNamespace([NotNull] this XDocument xDocument)
    {
        if (xDocument == null) throw new ArgumentNullException(nameof(xDocument));

        return new XDocument(xDocument.Root.OverrideNamespace(xDocument.Root.Name.Namespace));
    }

    private static XElement OverrideNamespace([NotNull] this XElement xElement, [NotNull] XNamespace xNamespace)
    {
        if (xElement == null) throw new ArgumentNullException(nameof(xElement));
        if (xNamespace == null) throw new ArgumentNullException(nameof(xNamespace));

        var newElement = new XElement(xNamespace + xElement.Name.LocalName);

        newElement.ReplaceAttributes(xElement.Attributes());
        newElement.ReplaceNodes(xElement.Elements().ToArray(child => child.OverrideNamespace(xNamespace)));

        return newElement;
    }

    public static XmlDocument ToXmlDocument(this XDocument xDocument)
    {
        var xmlDocument = new XmlDocument();

        using (var xmlReader = xDocument.CreateReader())
        {
            xmlDocument.Load(xmlReader);
        }

        return xmlDocument;
    }

    public static XDocument ToXDocument(this XmlDocument xmlDocument)
    {
        using (var nodeReader = new XmlNodeReader(xmlDocument))
        {
            nodeReader.MoveToContent();

            return XDocument.Load(nodeReader);
        }
    }
}
