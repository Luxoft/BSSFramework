using System.Linq.Expressions;

using CommonFramework;

using Framework.Core.Visitors;

namespace Framework.Projection.Environment;

public class DomainTypeExtendedMetadataBuilder<TDomainType> : IDomainTypeExtendedMetadataBuilder<TDomainType>, IDomainTypeExtendedMetadata
{
    private readonly List<Attribute> attributes = new ();

    private readonly List<IPropertyExtendedMetadata> properties = new ();


    public Type DomainType { get; } = typeof(TDomainType);

    public IReadOnlyList<Attribute> Attributes => this.attributes;

    public IReadOnlyList<IPropertyExtendedMetadata> Properties => this.properties;

    public IDomainTypeExtendedMetadataBuilder<TDomainType> AddAttribute(Attribute attribute)
    {
        this.attributes.Add(attribute);

        return this;
    }

    public IDomainTypeExtendedMetadataBuilder<TDomainType> AddProperty<TProperty>(Expression<Func<TDomainType, TProperty>> propertySelector, Action<IPropertyExtendedMetadataBuilder> setupAction)
    {
        var property = propertySelector.UpdateBody(FixPropertySourceVisitor.Value).GetProperty();

        var propertyBuilder = new PropertyExtendedMetadataBuilder(property);

        setupAction(propertyBuilder);

        this.properties.Add(propertyBuilder);

        return this;
    }
}
