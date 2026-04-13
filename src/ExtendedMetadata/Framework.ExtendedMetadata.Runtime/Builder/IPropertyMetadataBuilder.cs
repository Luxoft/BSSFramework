namespace Framework.ExtendedMetadata.Builder;

public interface IPropertyMetadataBuilder
{
    IPropertyMetadataBuilder AddAttribute(Attribute attribute);

    IPropertyMetadataBuilder AddAttribute<TAttribute>()
        where TAttribute : Attribute, new() => this.AddAttribute(new TAttribute());
}
