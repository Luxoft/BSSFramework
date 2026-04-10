using System.Collections.Immutable;
using System.Reflection;

namespace Framework.ExtendedMetadata.Builder;

public class PropertyMetadataBuilder(PropertyInfo property) : IPropertyMetadataBuilder
{
    public PropertyInfo Property { get; } = property;

    public ImmutableList<Attribute> Attributes { get; private set; } = [];

    public IPropertyMetadataBuilder AddAttribute(Attribute attribute)
    {
        this.Attributes = this.Attributes.Add(attribute);

        return this;
    }
}
