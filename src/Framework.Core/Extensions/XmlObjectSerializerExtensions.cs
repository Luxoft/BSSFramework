using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace Framework.Core
{
    public static class XmlObjectSerializerExtensions
    {
        public static string WriteToString(this XmlObjectSerializer serializer, object value, Encoding encoding = null)
        {
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));
            //if (value == null) throw new ArgumentNullException("value");

            var stringBuilder = new StringBuilder();

            using (var writer = XmlWriter.Create(stringBuilder, new XmlWriterSettings { Indent = true, Encoding = encoding ?? Encoding.Unicode }))
            {
                serializer.WriteObject(writer, value);
            }

            return stringBuilder.ToString();
        }

        public static void WriteToFile(this XmlObjectSerializer serializer, string path, object value, Encoding encoding = null)
        {
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));
            if (path == null) throw new ArgumentNullException(nameof(path));

            File.WriteAllText(path, serializer.WriteToString(value), encoding);
        }

        public static object ReadFromFile(this XmlObjectSerializer serializer, string path)
        {
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));
            if (path == null) throw new ArgumentNullException(nameof(path));

            using (var stream = File.OpenRead(path))
            {
                return serializer.ReadObject(stream);
            }
        }

        public static T ReadFromFile<T>(this XmlObjectSerializer serializer, string path)
        {
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));
            if (path == null) throw new ArgumentNullException(nameof(path));

            return (T)serializer.ReadFromFile(path);
        }

        public static object ReadFromString(this XmlObjectSerializer serializer, string value)
        {
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));
            if (value == null) throw new ArgumentNullException(nameof(value));


            using (var stringReader = new StringReader(value))
            using (var xmlReader = XmlReader.Create(stringReader))
            {
                return serializer.ReadObject(xmlReader);
            }
        }

        public static object ReadFromBinary(this XmlObjectSerializer serializer, byte[] value)
        {
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));
            if (value == null) throw new ArgumentNullException(nameof(value));

            using (var stream = new MemoryStream(value))
            {
                return serializer.ReadObject(stream);
            }
        }

        public static T ReadFromString<T>(this XmlObjectSerializer serializer, string value)
        {
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));
            if (value == null) throw new ArgumentNullException(nameof(value));

            return (T)serializer.ReadFromString(value);
        }

        public static T ReadFromBinary<T>(this XmlObjectSerializer serializer, byte[] value)
        {
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));
            if (value == null) throw new ArgumentNullException(nameof(value));

            return (T)serializer.ReadFromBinary(value);
        }
    }
}