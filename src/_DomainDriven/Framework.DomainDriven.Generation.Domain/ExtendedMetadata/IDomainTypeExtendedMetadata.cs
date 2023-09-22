namespace Framework.DomainDriven.Generation.Domain.ExtendedMetadata;

public interface IDomainTypeExtendedMetadata : IAttributesExtendedMetadata
{
    Type DomainType { get; }

    IReadOnlyList<IPropertyExtendedMetadata> Properties { get; }
}
