using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

using Framework.Core.Serialization;

namespace Framework.Core;

public static class DataContractSerializerHelper
{
    public static T Deserialize<T>(string source)
    {
        var serializer = new DataContractSerializer(typeof(T));

        using (var textReader = new StringReader (source))
            using (var xmlReader = new XmlTextReader(textReader))
            {
                return (T)serializer.ReadObject(xmlReader);
            }
    }

    public static string Serialize<T>(T source, Encoding encoding = null)
    {
        var serializer = new DataContractSerializer(typeof(T) == typeof(object) ? source.GetType() : typeof(T));

        var sb = new StringBuilder();
        using (var stringWriter = new EncodingStringWriter(sb, encoding))
            using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true, Encoding = encoding ?? Encoding.Unicode }))
            {
                serializer.WriteObject(xmlWriter, source);

                xmlWriter.Flush();

                return sb.ToString();
            }
    }


    public static T XmlClone<T>(this T source)
    {
        var serializer = new DataContractSerializer(typeof(T));

        using (var stream = new MemoryStream())
        {
            serializer.WriteObject(stream, source);

            stream.Position = 0;

            return (T)serializer.ReadObject(stream);
        }
    }

    public static bool XmlEquals<T>(this T source, T other)
    {
        var serializer = new DataContractSerializer(typeof(T));

        using (var stream1 = new MemoryStream())
        {
            serializer.WriteObject(stream1, source);

            using (var stream2 = new MemoryStream())
            {
                serializer.WriteObject(stream2, other);

                return stream1.ToArray().SequenceEqual(stream2.ToArray());
            }
        }
    }


    public static ISerializer<string, T> GetSerializer<T>()
    {
        return new Serializer<string, T> (Deserialize<T>, value => Serialize(value));
    }
}
