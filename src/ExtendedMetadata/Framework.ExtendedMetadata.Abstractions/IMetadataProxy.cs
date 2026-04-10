using System.Reflection;

namespace Framework.ExtendedMetadata;

public interface IMetadataProxy<out T> : ICustomAttributeProvider
{
    T Native { get; }

    T Wrapped { get; }

    IMetadataProxyProvider Provider { get; }
}
