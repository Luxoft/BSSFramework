using System.Reflection;

namespace Framework.ExtendedMetadata;

public class MetadataProxyProvider : IMetadataProxyProvider
{
    public IMetadataProxy<T> GetProxy<T>(T value)
        where T : ICustomAttributeProvider =>
        new MetadataProxy<T>(value, this);
}
