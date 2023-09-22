using System.Linq.Expressions;

namespace Framework.DomainDriven.Generation.Domain.ExtendedMetadata;

public interface IDomainTypeExtendedMetadataBuilder<TDomainType>
{
    IDomainTypeExtendedMetadataBuilder<TDomainType> AddAttribute(Attribute attribute);

    IDomainTypeExtendedMetadataBuilder<TDomainType> AddProperty<TProperty>(
        Expression<Func<TDomainType, TProperty>> propertySelector,
        Action<IPropertyExtendedMetadataBuilder> setupAction);
}
