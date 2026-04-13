using System.Collections.Immutable;
using System.Linq.Expressions;

using CommonFramework;

using Framework.Core.Visitors;

namespace Framework.ExtendedMetadata.Builder;

public class DomainTypeMetadataBuilder<TDomainType> : IDomainTypeMetadataBuilder<TDomainType>, IDomainTypeMetadataBuilder
{
    public ImmutableList<Attribute> Attributes { get; private set; } = [];

    public ImmutableList<PropertyMetadataBuilder> Properties { get; private set; } = [];

    public IDomainTypeMetadataBuilder<TDomainType> AddAttribute(Attribute attribute)
    {
        this.Attributes = this.Attributes.Add(attribute);

        return this;
    }

    public IDomainTypeMetadataBuilder<TDomainType> AddProperty<TProperty>(Expression<Func<TDomainType, TProperty>> propertySelector, Action<IPropertyMetadataBuilder> setupAction)
    {
        var property = propertySelector.UpdateBody(FixPropertySourceVisitor.Value).GetProperty();

        var propertyBuilder = new PropertyMetadataBuilder(property);

        setupAction(propertyBuilder);

        this.Properties = this.Properties.Add(propertyBuilder);

        return this;
    }
}
