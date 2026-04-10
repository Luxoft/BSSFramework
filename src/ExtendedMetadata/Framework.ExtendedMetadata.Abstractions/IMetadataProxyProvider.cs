using System.Reflection;

namespace Framework.ExtendedMetadata;

public interface IMetadataProxyProvider
{
    IMetadataProxy<T> GetProxy<T>(T value)
        where T : ICustomAttributeProvider;

    T Wrap<T>(T value)
        where T : ICustomAttributeProvider;
}
