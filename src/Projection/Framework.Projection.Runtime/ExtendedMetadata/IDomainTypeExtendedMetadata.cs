namespace Framework.Projection.ExtendedMetadata;

public interface IDomainTypeExtendedMetadata : IAttributesExtendedMetadata
{
    Type DomainType { get; }

    IReadOnlyList<IPropertyExtendedMetadata> Properties { get; }
}
