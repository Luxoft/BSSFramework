using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Reflection;

using CommonFramework;

using Framework.Core;

namespace Framework.Projection.Environment;

public class DomainTypeRootExtendedMetadataBuilder : IDomainTypeRootExtendedMetadata
{
    private readonly Dictionary<Type, IDomainTypeExtendedMetadata> types = new();

    private readonly ConcurrentDictionary<Type, ICustomAttributeProvider> typeCache = [];

    private readonly ConcurrentDictionary<PropertyInfo, ICustomAttributeProvider> propertyCache = [];

    private IEnumerable<IDomainTypeExtendedMetadata> GetOrderedExtendedMetadata(Type type) =>
        this.types
            .Where(t => t.Key.IsAssignableFrom(type))
            .OrderByDescending(pair => type.GetAllElements(t => t.BaseType).IndexOf(pair.Key))
            .Select(pair => pair.Value);

    public virtual ICustomAttributeProvider GetType(Type type) =>
        this.typeCache.GetOrAdd(
            type,
            _ =>
            {
                var usedTypes = this.GetOrderedExtendedMetadata(type).ToImmutableArray();

                if (usedTypes.Any())
                {
                    return new MixedCustomAttributeProvider<Type, IDomainTypeExtendedMetadata>(type, usedTypes);
                }
                else
                {
                    return type;
                }
            });

    public virtual ICustomAttributeProvider GetProperty(PropertyInfo property) =>
        this.propertyCache.GetOrAdd(
            property,
            _ =>
            {
                var usedProperties = this.GetOrderedExtendedMetadata(property.ReflectedType!)
                                         .SelectMany(v => v.Properties)
                                         .Where(prop => prop.Property.IsAssignableFrom(property))
                                         .OrderByDescending(propM => propM.Property.GetAllElements(v => v.GetBaseProperty()).IndexOf(propM.Property))
                                         .ToImmutableArray();

                if (usedProperties.Any())
                {
                    return new MixedCustomAttributeProvider<PropertyInfo, IPropertyExtendedMetadata>(property, usedProperties);
                }
                else
                {
                    return property;
                }
            });

    public DomainTypeRootExtendedMetadataBuilder Add<TDomainType>(Action<IDomainTypeExtendedMetadataBuilder<TDomainType>> setupAction)
    {
        var typeBuilder = new DomainTypeExtendedMetadataBuilder<TDomainType>();

        setupAction(typeBuilder);

        this.types.Add(typeof(TDomainType), typeBuilder);

        return this;
    }
}
