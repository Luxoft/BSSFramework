using Framework.Projection;

namespace Framework.DomainDriven.Generation.Domain;

public interface IGenerationEnvironment : IDomainMetadata
{
    string TargetSystemName { get; }

    Type SecurityOperationCodeType { get; }

    Type OperationContextType { get; }

    IReadOnlyCollection<IProjectionEnvironment> ProjectionEnvironments { get; }
}
