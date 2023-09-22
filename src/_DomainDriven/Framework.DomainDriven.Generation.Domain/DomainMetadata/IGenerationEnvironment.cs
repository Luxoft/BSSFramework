using Framework.DomainDriven.Generation.Domain.ExtendedMetadata;
using Framework.Projection;

namespace Framework.DomainDriven.Generation.Domain;

public interface IGenerationEnvironment : IDomainMetadata
{
    string TargetSystemName { get; }

    Type SecurityOperationType { get; }

    Type OperationContextType { get; }

    IReadOnlyCollection<IProjectionEnvironment> ProjectionEnvironments { get; }

    IDomainTypeRootExtendedMetadata ExtendedMetadata { get; }
}
