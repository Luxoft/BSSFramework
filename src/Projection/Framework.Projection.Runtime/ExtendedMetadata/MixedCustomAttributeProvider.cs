using System.Reflection;

namespace Framework.Projection.ExtendedMetadata;

public class MixedCustomAttributeProvider(ICustomAttributeProvider baseProvider, IAttributesExtendedMetadata attributesExtendedMetadata)
    : ICustomAttributeProvider
{
    public object[] GetCustomAttributes(bool inherit) =>
        baseProvider.GetCustomAttributes(inherit).Cast<Attribute>()
                    .Concat(attributesExtendedMetadata.Attributes)
                    .ToArray();

    public object[] GetCustomAttributes(Type attributeType, bool inherit) =>
        baseProvider.GetCustomAttributes(attributeType, inherit).Cast<Attribute>()
                    .Concat(attributesExtendedMetadata.Attributes.Where(attr => attributeType.IsAssignableFrom(attr.GetType())))
                    .ToArray();

    public bool IsDefined(Type attributeType, bool inherit) =>
        baseProvider.IsDefined(attributeType, inherit)
        || attributesExtendedMetadata.Attributes.Any(attr => attributeType.IsAssignableFrom(attr.GetType()));
}
