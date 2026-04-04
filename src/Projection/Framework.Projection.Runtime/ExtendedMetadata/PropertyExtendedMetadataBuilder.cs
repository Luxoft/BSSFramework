using System.Reflection;

namespace Framework.Projection.ExtendedMetadata;

public class PropertyExtendedMetadataBuilder(PropertyInfo property) : IPropertyExtendedMetadataBuilder, IPropertyExtendedMetadata
{
    private readonly List<Attribute> attributes = [];

    public PropertyInfo Property { get; } = property;

    public IReadOnlyList<Attribute> Attributes => this.attributes;

    public IPropertyExtendedMetadataBuilder AddAttribute(Attribute attribute)
    {
        this.attributes.Add(attribute);

        return this;
    }
}
