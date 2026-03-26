namespace Framework.Projection.Environment;

public interface IDomainTypeExtendedMetadata : IAttributesExtendedMetadata
{
    Type DomainType { get; }

    IReadOnlyList<IPropertyExtendedMetadata> Properties { get; }
}
