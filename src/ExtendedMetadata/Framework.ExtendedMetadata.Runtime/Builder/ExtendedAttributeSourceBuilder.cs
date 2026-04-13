using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Reflection;

namespace Framework.ExtendedMetadata.Builder;

public class ExtendedAttributeSourceBuilder : IExtendedAttributeSourceBuilder
{
    private readonly Dictionary<Type, IDomainTypeMetadataBuilder> types = [];


    public IExtendedAttributeSourceBuilder Add<TDomainType>(Action<IDomainTypeMetadataBuilder<TDomainType>> setupAction)
    {
        var typeBuilder = new DomainTypeMetadataBuilder<TDomainType>();

        setupAction(typeBuilder);

        this.types.Add(typeof(TDomainType), typeBuilder);

        return this;
    }

    public ExtendedAttributeSource Build() => new(this.GetExtendedAttributes().ToFrozenDictionary(v => v.Item1, v => v.Item2));

    private IEnumerable<(MemberInfo, ImmutableArray<Attribute>)> GetExtendedAttributes()
    {
        foreach (var domainTypeMetadataBuilder in this.types)
        {
            yield return (domainTypeMetadataBuilder.Key, [..domainTypeMetadataBuilder.Value.Attributes]);

            foreach (var propertyMetadataBuilder in domainTypeMetadataBuilder.Value.Properties)
            {
                yield return (propertyMetadataBuilder.Property, [.. propertyMetadataBuilder.Attributes]);
            }
        }
    }
}
