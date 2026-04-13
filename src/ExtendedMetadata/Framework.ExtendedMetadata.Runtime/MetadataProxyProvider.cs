using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Reflection;
using System.Reflection.Context;

using Framework.Core;

namespace Framework.ExtendedMetadata;

public class MetadataProxyProvider(IEnumerable<ExtendedAttributeSource> extendedAttributeSourceList) : CustomReflectionContext, IMetadataProxyProvider
{
    private readonly ExtendedAttributeSource extendedAttributeSource = new(extendedAttributeSourceList);

    private readonly ConcurrentDictionary<PropertyInfo, ImmutableArray<Attribute>> propCache = [];

    private readonly ConcurrentDictionary<Type, ImmutableArray<Attribute>> typeCache = [];

    public T Wrap<T>(T value)
        where T : class, ICustomAttributeProvider =>
        this.TryWrap(value) ?? value;

    public T? TryWrap<T>(T value)
        where T : class, ICustomAttributeProvider
    {
        if (value is IWrappingObject { CanWrap: false })
        {
            return null;
        }
        else if (value is Type type)
        {
            var proxyType = (T)(object)this.MapType(type.GetTypeInfo());

            if (proxyType.GetType() == value.GetType())
            {
                return null;
            }
            else
            {
                return proxyType;
            }
        }
        else if (value is PropertyInfo property)
        {
            var wrappedType = this.Wrap(property.ReflectedType!);

            var wrappedProperties = wrappedType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            var proxyProp = (T)(object)wrappedProperties.Single(prop => prop.Name == property.Name);

            if (proxyProp.GetType() == value.GetType())
            {
                return null;
            }
            else
            {
                return proxyProp;
            }
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }
    }

    protected override IEnumerable<object> GetCustomAttributes(MemberInfo member, IEnumerable<object> declaredAttributes)
    {
        if (member is PropertyInfo prop)
        {
            return this.GetCustomAttributes(prop);
        }
        else if (member is Type type)
        {
            var baseAttributes = base.GetCustomAttributes(member, declaredAttributes);

            var newAttributes = this.GetExtendedAttributes(type);

            return [.. newAttributes, .. baseAttributes];
        }
        else
        {
            return base.GetCustomAttributes(member, declaredAttributes);
        }
    }

    private ImmutableArray<Attribute> GetExtendedAttributes(Type type) =>

        this.typeCache.GetOrAdd(type, _ => this.extendedAttributeSource.ExtendedAttributes.GetValueOrDefault(type, []));

    private ImmutableArray<Attribute> GetCustomAttributes(PropertyInfo prop) =>

        this.propCache.GetOrAdd(
            prop,
            _ =>
            {
                var baseAttributes = prop.GetCustomAttributes();

                var newAttributes = prop.GetBaseProperties().SelectMany(p => this.extendedAttributeSource.ExtendedAttributes.GetValueOrDefault(p, [])).Distinct();

                return [.. newAttributes, .. baseAttributes];
            });

}
