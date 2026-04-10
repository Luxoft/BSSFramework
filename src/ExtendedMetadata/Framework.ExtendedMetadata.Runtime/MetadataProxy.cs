using System.Reflection;

namespace Framework.ExtendedMetadata;

public class MetadataProxy<T>(T native, IMetadataProxyProvider provider) : IMetadataProxy<T>
    where T : ICustomAttributeProvider
{
    public T Native => native;

    public T Wrapped => provider.Wrap(this.Native);

    public IMetadataProxyProvider Provider => provider;

    public object[] GetCustomAttributes(bool inherit) => this.Wrapped.GetCustomAttributes(inherit);

    public object[] GetCustomAttributes(Type attributeType, bool inherit) => this.Wrapped.GetCustomAttributes(attributeType, inherit);

    public bool IsDefined(Type attributeType, bool inherit) => this.Wrapped.IsDefined(attributeType, inherit);
}
