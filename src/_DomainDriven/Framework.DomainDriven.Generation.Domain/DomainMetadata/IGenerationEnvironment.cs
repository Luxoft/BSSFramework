using Framework.Projection;
using Framework.Projection.Environment;

namespace Framework.DomainDriven.Generation.Domain;

public interface IGenerationEnvironment : IDomainMetadata
{
    string TargetSystemName { get; }

    Type SecurityOperationType { get; }

    Type OperationContextType { get; }

    IReadOnlyCollection<IProjectionEnvironment> ProjectionEnvironments { get; }

    IDomainTypeRootExtendedMetadata ExtendedMetadata { get; }
}
