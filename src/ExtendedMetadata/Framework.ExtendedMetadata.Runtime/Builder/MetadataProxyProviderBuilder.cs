using System.Collections.Immutable;
using System.Reflection;

namespace Framework.ExtendedMetadata.Builder;

public class MetadataProxyProviderBuilder : IMetadataProxyProviderBuilder
{
    private readonly Dictionary<Type, IDomainTypeMetadataBuilder> types = [];


    public IMetadataProxyProviderBuilder Add<TDomainType>(Action<IDomainTypeMetadataBuilder<TDomainType>> setupAction)
    {
        var typeBuilder = new DomainTypeMetadataBuilder<TDomainType>();

        setupAction(typeBuilder);

        this.types.Add(typeof(TDomainType), typeBuilder);

        return this;
    }

    public IMetadataProxyProvider Build() => new MetadataProxyProvider(this.GetExtendedAttributes().ToDictionary());

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
