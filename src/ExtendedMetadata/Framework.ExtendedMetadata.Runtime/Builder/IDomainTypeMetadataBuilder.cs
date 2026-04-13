using System.Collections.Immutable;
using System.Linq.Expressions;

namespace Framework.ExtendedMetadata.Builder;

public interface IDomainTypeMetadataBuilder
{
    ImmutableList<Attribute> Attributes { get; }

    ImmutableList<PropertyMetadataBuilder> Properties { get; }
}

public interface IDomainTypeMetadataBuilder<TDomainType>
{
    IDomainTypeMetadataBuilder<TDomainType> AddAttribute(Attribute attribute);

    IDomainTypeMetadataBuilder<TDomainType> AddAttribute<TAttribute>()
        where TAttribute : Attribute, new() => this.AddAttribute(new TAttribute());

    IDomainTypeMetadataBuilder<TDomainType> AddProperty<TProperty>(
        Expression<Func<TDomainType, TProperty>> propertySelector,
        Action<IPropertyMetadataBuilder> setupAction);
}
