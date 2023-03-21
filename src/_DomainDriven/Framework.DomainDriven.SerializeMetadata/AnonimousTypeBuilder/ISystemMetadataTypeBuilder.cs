using Framework.Core;

namespace Framework.DomainDriven.SerializeMetadata;

public interface ISystemMetadataTypeBuilder : ITypeResolverContainer<TypeHeader>, IIAnonymousTypeBuilderContainer<DomainTypeSubsetMetadata>
{
    SystemMetadata SystemMetadata { get; }
}
