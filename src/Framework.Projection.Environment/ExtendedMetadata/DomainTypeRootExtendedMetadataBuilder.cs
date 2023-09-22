using System.Reflection;

using Framework.Core;

namespace Framework.Projection.Environment;

public class DomainTypeRootExtendedMetadataBuilder : IDomainTypeRootExtendedMetadata
{
    private readonly Dictionary<Type, IDomainTypeExtendedMetadata> types = new();

    private readonly Lazy<Dictionary<PropertyInfo, IPropertyExtendedMetadata>> properties = new();

    public DomainTypeRootExtendedMetadataBuilder()
    {
        this.properties = LazyHelper.Create(() => this.types.SelectMany(t => t.Value.Properties).ToDictionary(prop => prop.Property));
    }

    public virtual ICustomAttributeProvider GetType(Type type)
    {
        if (this.types.TryGetValue(type, out var metadata))
        {
            return new MixedCustomAttributeProvider(type, metadata);
        }
        else
        {
            return type;
        }
    }

    public virtual ICustomAttributeProvider GetProperty(PropertyInfo property)
    {
        if (this.properties.Value.TryGetValue(property, out var propertyMetadata))
        {
            return new MixedCustomAttributeProvider(property, propertyMetadata);
        }
        else
        {
            return property;
        }
    }

    public DomainTypeRootExtendedMetadataBuilder Add<TDomainType>(Action<IDomainTypeExtendedMetadataBuilder<TDomainType>> setupAction)
    {
        var typeBuilder = new DomainTypeExtendedMetadataBuilder<TDomainType>();

        setupAction(typeBuilder);

        this.types.Add(typeof(TDomainType), typeBuilder);

        return this;
    }
}
