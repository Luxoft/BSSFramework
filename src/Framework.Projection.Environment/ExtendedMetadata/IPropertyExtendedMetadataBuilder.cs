namespace Framework.Projection.Environment;

public interface IPropertyExtendedMetadataBuilder
{
    IPropertyExtendedMetadataBuilder AddAttribute(Attribute attribute);

    IPropertyExtendedMetadataBuilder AddAttribute<TAttribute>()
        where TAttribute : Attribute, new() => this.AddAttribute(new TAttribute());
}
