using System.Reflection;

namespace Framework.ExtendedMetadata;

public interface IMetadataProxy<out T> : ICustomAttributeProvider
{
    T Native { get; }

    IMetadataProxyProvider Provider { get; }
}
