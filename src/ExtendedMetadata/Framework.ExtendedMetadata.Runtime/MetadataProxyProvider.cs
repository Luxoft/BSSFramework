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

            var prop = wrappedProperties.Single(prop => prop.Name == property.Name);

            if (prop.Name == "Name")
            {

            }

            return (T)(object)prop;
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }
    }

    protected override IEnumerable<object> GetCustomAttributes(MemberInfo member, IEnumerable<object> declaredAttributes)
    {
        var newAttributes = extendedAttributes.GetValueOrDefault(member, []);

        var baseAttributes = member is PropertyInfo prop
                                 ? Attribute.GetCustomAttributes(prop)
                                 : base.GetCustomAttributes(member, declaredAttributes);

        return [.. newAttributes, .. baseAttributes];
    }
}
