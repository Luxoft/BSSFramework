using System.Reflection;

namespace Framework.DomainDriven.Generation.Domain.ExtendedMetadata;

public class PropertyExtendedMetadataBuilder : IPropertyExtendedMetadataBuilder, IPropertyExtendedMetadata
{
    private readonly List<Attribute> attributes = new ();

    public PropertyExtendedMetadataBuilder(PropertyInfo property)
    {
        this.Property = property;
    }

    public PropertyInfo Property { get; }

    public IReadOnlyList<Attribute> Attributes => this.attributes;

    public IPropertyExtendedMetadataBuilder AddAttribute(Attribute attribute)
    {
        this.attributes.Add(attribute);

        return this;
    }
}
