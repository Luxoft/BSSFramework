using System.Collections.Immutable;
using System.Reflection;
using System.Reflection.Context;

namespace Framework.ExtendedMetadata;

public class MetadataProxyProvider(IReadOnlyDictionary<MemberInfo, ImmutableArray<Attribute>> extendedAttributes) : CustomReflectionContext, IMetadataProxyProvider
{
    public IMetadataProxy<T> GetProxy<T>(T value)
        where T : ICustomAttributeProvider =>
        new MetadataProxy<T>(value, this);

    public T Wrap<T>(T value)
        where T : ICustomAttributeProvider
    {
        if (value is Type type)
        {
            return (T)(object)this.MapType(type.GetTypeInfo());
        }
        else if (value is PropertyInfo property)
        {
            var wrappedType = this.Wrap(property.ReflectedType!);

            var wrappedProperties = wrappedType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            return (T)(object)wrappedProperties.Single(prop => prop.Name == property.Name);
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }
    }

    protected override IEnumerable<object> GetCustomAttributes(MemberInfo member, IEnumerable<object> declaredAttributes) =>

        [.. extendedAttributes.GetValueOrDefault(member, []), .. base.GetCustomAttributes(member, declaredAttributes)];
}
