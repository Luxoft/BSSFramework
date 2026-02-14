using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Reflection;

using Framework.Core;

namespace Framework.Projection.Environment;

public class MixedCustomAttributeProvider<TBaseProvider, TAttributesExtendedMetadata>(
    TBaseProvider baseProvider,
    ImmutableArray<TAttributesExtendedMetadata> attributesExtendedMetadataList)
    : ICustomAttributeProvider
    where TBaseProvider : ICustomAttributeProvider
    where TAttributesExtendedMetadata : IAttributesExtendedMetadata
{
    private readonly ConcurrentDictionary<bool, ImmutableArray<object>> simpleCustomAttributesCache = [];

    private readonly ConcurrentDictionary<(Type, bool), ImmutableArray<object>> customAttributesCache = [];

    private readonly ConcurrentDictionary<(Type, bool), bool> isDefinedCache = [];

    private IEnumerable<Attribute> GetMetadataAttributes(Type attributeType)
    {
        var allowMultiple = attributeType.HasAttribute<AttributeUsageAttribute>(a => a.AllowMultiple);

        return from attributesExtendedMetadata in attributesExtendedMetadataList

               from attr in attributesExtendedMetadata.Attributes

               where attributeType.IsInstanceOfType(attr)

               group attr by attr.GetType()

               into attrGroup

               from attr in allowMultiple ? attrGroup.Select(v => v) : [attrGroup.First()]

               select attr;
    }

    public object[] GetCustomAttributes(bool inherit) =>
        this.simpleCustomAttributesCache.GetOrAdd(
            inherit,
            _ => baseProvider.GetCustomAttributes(inherit)
                             .Cast<Attribute>()
                             .Concat(this.GetMetadataAttributes(typeof(Attribute)))
                             .ToImmutableArray<object>())
            .ToArray();

    public object[] GetCustomAttributes(Type attributeType, bool inherit) =>
        this.customAttributesCache.GetOrAdd(
                (attributeType, inherit),
                _ => baseProvider.GetCustomAttributes(attributeType, inherit)
                                 .Cast<Attribute>()
                                 .Concat(this.GetMetadataAttributes(attributeType))
                                 .ToImmutableArray<object>())
            .ToArray();

    public bool IsDefined(Type attributeType, bool inherit) =>
        this.isDefinedCache.GetOrAdd(
            (attributeType, inherit),
            _ => baseProvider.IsDefined(attributeType, inherit)
                 || this.GetMetadataAttributes(attributeType).Any(attributeType.IsInstanceOfType));
}
