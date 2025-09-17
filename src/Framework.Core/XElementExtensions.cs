using System.Xml.Linq;

namespace Framework.Core;

public static class XElementExtensions
{
    public static XElement CreateElement(this XElement source, string name)
    {
        return source.CreateElement(XName.Get(name));
    }
    public static void CreateElement(this XElement source, XElement element)
    {
        source.Add(element);
    }
    public static XElement CreateElement(this XElement source, XName xName)
    {
        var newElement = new XElement(xName);
        source.Add(newElement);
        return newElement;
    }

    public static XElement WithAttribute(this XElement source, string name, object value)
    {
        return source.WithAttribute(XName.Get(name), value);
    }
    public static XElement MaybeWithAttribute(this XElement source, string name, object value)
    {
        if (null != value)
        {
            source.WithAttribute(XName.Get(name), value);
        }
        return source;
    }
    public static XElement WithAttribute(this XElement source, XName name, object value)
    {
        source.Add(new XAttribute(name, value));
        return source;
    }

    public static XElement WithNameAttribute(this XElement source, object value)
    {
        source.WithAttribute("Name", value);
        return source;
    }
    public static XElement WithLengthAttribute(this XElement source, int value)
    {
        source.WithAttribute("length", value);
        return source;
    }

    public static XElement WithLowNameAttribute(this XElement source, object value)
    {
        source.WithAttribute("name", value);
        return source;
    }

    public static XElement WithColumnNameAttribute(this XElement source, object value)
    {
        source.Add(new XAttribute("ColumnName", value));
        return source;
    }
    public static XElement CreateElementWithRootNamespace(this XElement source, string name)
    {
        var newElement = new XElement(source.Name.Namespace + name);
        source.Add(newElement);
        return newElement;
    }
}
