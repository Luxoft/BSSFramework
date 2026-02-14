using System.Linq.Expressions;

namespace Framework.Projection.Environment;

public interface IDomainTypeExtendedMetadataBuilder<TDomainType>
{
    IDomainTypeExtendedMetadataBuilder<TDomainType> AddAttribute(Attribute attribute);

    IDomainTypeExtendedMetadataBuilder<TDomainType> AddAttribute<TAttribute>()
        where TAttribute : Attribute, new() => this.AddAttribute(new TAttribute());

    IDomainTypeExtendedMetadataBuilder<TDomainType> AddProperty<TProperty>(
        Expression<Func<TDomainType, TProperty>> propertySelector,
        Action<IPropertyExtendedMetadataBuilder> setupAction);
}
