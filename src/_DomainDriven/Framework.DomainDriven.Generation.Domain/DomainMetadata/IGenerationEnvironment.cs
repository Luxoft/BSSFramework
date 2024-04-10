using Framework.Projection;
using Framework.Projection.Environment;

namespace Framework.DomainDriven.Generation.Domain;

public interface IGenerationEnvironment : IDomainMetadata
{
    string TargetSystemName { get; }

    IReadOnlyList<Type> SecurityRuleTypeList { get; }

    Type OperationContextType { get; }

    IReadOnlyCollection<IProjectionEnvironment> ProjectionEnvironments { get; }

    IDomainTypeRootExtendedMetadata ExtendedMetadata { get; }
}
