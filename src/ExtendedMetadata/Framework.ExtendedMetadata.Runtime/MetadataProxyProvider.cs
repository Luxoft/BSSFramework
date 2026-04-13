using System.Collections.Immutable;
using System.Reflection;
using System.Reflection.Context;

using Framework.Core;

namespace Framework.ExtendedMetadata;

public class MetadataProxyProvider(IReadOnlyDictionary<MemberInfo, ImmutableArray<Attribute>> extendedAttributes) : CustomReflectionContext, IMetadataProxyProvider
{
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
            return (T)(object)this.MapType(type.GetTypeInfo());
        }
        else if (value is PropertyInfo property)
        {
            var wrappedType = this.Wrap(property.ReflectedType!);

            var wrappedProperties = wrappedType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            var prop = wrappedProperties.Single(prop => prop.Name == property.Name);

            return (T)(object)prop;
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }
    }

    protected override IEnumerable<object> GetCustomAttributes(MemberInfo member, IEnumerable<object> declaredAttributes) //=>

        //this.cache.GetOrAdd(
          //  member,
            //_ =>
            {
                if (member is PropertyInfo prop)
                {
                    var baseAttributes = prop.GetCustomAttributes();

                    var newAttributes = prop.GetBaseProperties().SelectMany(p => extendedAttributes.GetValueOrDefault(p, [])).Distinct();

                    return [.. newAttributes, .. baseAttributes];
                }
                else
                {
                    var baseAttributes = base.GetCustomAttributes(member, declaredAttributes);

                    var newAttributes = extendedAttributes.GetValueOrDefault(member, []);

                    return [.. newAttributes, .. baseAttributes];
                }
            }
            //);
}
