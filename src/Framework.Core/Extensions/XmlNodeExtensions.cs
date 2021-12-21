using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Framework.Core
{
    public static class XmlNodeExtensions
    {
        public static XmlNode GetByName(this IEnumerable<XmlNode> xmlNodes, string name)
        {
            return xmlNodes.SingleOrDefault(node => node.Name == name);
        }


        public static XmlNode[] GetChildren(this XmlNode xmlNode)
        {
            if (xmlNode == null) throw new ArgumentNullException(nameof(xmlNode));

            return xmlNode.HasChildNodes
                 ? xmlNode.FirstChild.GetAllElements(c => c.NextSibling).ToArray()
                 : new XmlNode[0];
        }

        public static XmlAttribute[] GetAttributes(this XmlNode xmlNode)
        {
            if (xmlNode == null) throw new ArgumentNullException(nameof(xmlNode));

            return xmlNode.Attributes == null
                ? new XmlAttribute[0]
                : xmlNode.Attributes.Cast<XmlAttribute>().ToArray();
        }

        public static XmlNode AppendChild(this XmlNode xmlNode, string name)
        {
            if (xmlNode == null) throw new ArgumentNullException(nameof(xmlNode));
            if (name == null) throw new ArgumentNullException(nameof(name));

            return xmlNode.AppendChild(xmlNode.OwnerDocument.CreateElement(name));
        }

        public static XmlAttribute GetOrCreateAttribute(this XmlNode xmlNode, string name)
        {
            if (xmlNode == null) throw new ArgumentNullException(nameof(xmlNode));
            if (name == null) throw new ArgumentNullException(nameof(name));

            return xmlNode.Attributes[name] ?? xmlNode.CreateAttribute(name);
        }

        public static XmlAttribute CreateAttribute(this XmlNode xmlNode, string name)
        {
            if (xmlNode == null) throw new ArgumentNullException(nameof(xmlNode));
            if (name == null) throw new ArgumentNullException(nameof(name));

            return xmlNode.Attributes.Append(xmlNode.OwnerDocument.CreateAttribute(name));
        }

        public static XmlAttribute CreateAttribute(this XmlNode xmlNode, string name, string value)
        {
            if (xmlNode == null) throw new ArgumentNullException(nameof(xmlNode));
            if (name == null) throw new ArgumentNullException(nameof(name));

            return xmlNode.CreateAttribute(name).Self(node => node.Value = value);
        }


        public static void ReplaceChildrenNodes(this XmlNode toNode, XmlNode fromNode)
        {
            if (toNode == null) throw new ArgumentNullException(nameof(toNode));
            if (fromNode == null) throw new ArgumentNullException(nameof(fromNode));

            toNode.GetChildren().Foreach(node => toNode.RemoveChild(node));

            var newChildren = fromNode.GetChildren();

            newChildren.Foreach(node =>
            {
                var importedNode = toNode.OwnerDocument.ImportNode(node, true);

                toNode.AppendChild(importedNode);
            });
        }
    }
}