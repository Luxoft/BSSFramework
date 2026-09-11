using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

using Framework.Core.Serialization;

namespace Framework.Core.Helpers;

public static class DataContractSerializerHelper
{
    private static readonly DataContractSerializerSettings ProxyTolerantSettings = new() { DataContractResolver = DynamicProxyDataContractResolver.Instance };

    public static T Deserialize<T>(string source)
    {
        var serializer = new DataContractSerializer(typeof(T), ProxyTolerantSettings);

        using var textReader = new StringReader(source);
        using var xmlReader = new XmlTextReader(textReader);
        return (T)serializer.ReadObject(xmlReader)!;
    }

    public static string Serialize<T>(T source, Encoding? encoding = null)
    {
        var serializer = new DataContractSerializer(typeof(T) == typeof(object) ? source!.GetType() : typeof(T), ProxyTolerantSettings);

        var sb = new StringBuilder();
        var resolvedEncoding = encoding ?? Encoding.Unicode;
        using var stringWriter = new EncodingStringWriter(sb, resolvedEncoding);
        using var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true, Encoding = resolvedEncoding });
        serializer.WriteObject(xmlWriter, source);

        xmlWriter.Flush();

        return sb.ToString();
    }


    public static T XmlClone<T>(this T source)
    {
        var serializer = new DataContractSerializer(typeof(T), ProxyTolerantSettings);

        using var stream = new MemoryStream();
        serializer.WriteObject(stream, source);

        stream.Position = 0;

        return (T)serializer.ReadObject(stream)!;
    }

    public static bool XmlEquals<T>(this T source, T other)
    {
        var serializer = new DataContractSerializer(typeof(T), ProxyTolerantSettings);

        using var stream1 = new MemoryStream();
        serializer.WriteObject(stream1, source);

        using var stream2 = new MemoryStream();
        serializer.WriteObject(stream2, other);

        return stream1.ToArray().SequenceEqual(stream2.ToArray());
    }


    public static ISerializer<string, T> GetSerializer<T>() => new Serializer<string, T>(Deserialize<T>, value => Serialize(value));

    /// <summary>
    /// Falls back to the declared type when the runtime type is an unrecognized dynamic proxy (e.g. Castle DynamicProxy),
    /// so lazy-loading/change-tracking proxy instances that leak into a serialized graph don't break DataContractSerializer.
    /// </summary>
    private sealed class DynamicProxyDataContractResolver : DataContractResolver
    {
        public static readonly DataContractResolver Instance = new DynamicProxyDataContractResolver();

        private static readonly XmlDictionary Dictionary = new();

        private static readonly ConcurrentDictionary<Type, (XmlDictionaryString Name, XmlDictionaryString Namespace)> ContractNameCache = new();

        public override bool TryResolveType(Type dataContractType, Type declaredType, DataContractResolver? knownTypeResolver, out XmlDictionaryString typeName, out XmlDictionaryString typeNamespace)
        {
            var realType = dataContractType.Namespace == "Castle.Proxies" ? dataContractType.BaseType ?? dataContractType : dataContractType;

            if (realType != dataContractType)
            {
                (typeName, typeNamespace) = ContractNameCache.GetOrAdd(realType, GetContractName);
                return true;
            }

            typeName = null!;
            typeNamespace = null!;
            return knownTypeResolver!.TryResolveType(dataContractType, declaredType, null, out typeName, out typeNamespace);
        }

        public override Type ResolveName(string typeName, string? typeNamespace, Type declaredType, DataContractResolver? knownTypeResolver) =>
            knownTypeResolver!.ResolveName(typeName, typeNamespace, declaredType, null);

        private static (XmlDictionaryString Name, XmlDictionaryString Namespace) GetContractName(Type type)
        {
            var contract = type.GetCustomAttribute<DataContractAttribute>(inherit: true);
            var name = contract is { IsNameSetExplicitly: true } ? contract.Name! : type.Name;
            var ns = contract is { IsNamespaceSetExplicitly: true } ? contract.Namespace! : "http://schemas.datacontract.org/2004/07/" + type.Namespace;

            return (Dictionary.Add(name), Dictionary.Add(ns));
        }
    }
}
