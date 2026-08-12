using System.Reflection;

namespace Framework.ExtendedMetadata;

public interface IMetadataProxyProvider
{
    T Wrap<T>(T value)
        where T : class, ICustomAttributeProvider;

    T? TryWrap<T>(T value)
        where T : class, ICustomAttributeProvider;
}
