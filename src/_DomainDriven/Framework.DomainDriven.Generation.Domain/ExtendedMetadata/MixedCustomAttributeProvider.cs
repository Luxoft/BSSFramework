using System.Reflection;

namespace Framework.DomainDriven.Generation.Domain.ExtendedMetadata;

public class MixedCustomAttributeProvider : ICustomAttributeProvider
{
    private readonly ICustomAttributeProvider baseProvider;

    private readonly IAttributesExtendedMetadata attributesExtendedMetadata;

    public MixedCustomAttributeProvider(ICustomAttributeProvider baseProvider, IAttributesExtendedMetadata attributesExtendedMetadata)
    {
        this.baseProvider = baseProvider;
        this.attributesExtendedMetadata = attributesExtendedMetadata;
    }

    public object[] GetCustomAttributes(bool inherit)
    {
        return this.baseProvider.GetCustomAttributes(inherit).Cast<Attribute>()
                   .Concat(this.attributesExtendedMetadata.Attributes)
                   .ToArray();
    }

    public object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
        return this.baseProvider.GetCustomAttributes(attributeType, inherit).Cast<Attribute>()
                   .Concat(this.attributesExtendedMetadata.Attributes.Where(attr => attributeType.IsAssignableFrom(attr.GetType())))
                   .ToArray();
    }

    public bool IsDefined(Type attributeType, bool inherit)
    {
        return this.baseProvider.IsDefined(attributeType, inherit)
               || this.attributesExtendedMetadata.Attributes.Any(attr => attributeType.IsAssignableFrom(attr.GetType()));
    }
}
