using Framework.Core;
using Framework.Projection;
using Framework.Projection.Environment;

namespace Framework.CodeGeneration.DomainMetadata;

public interface IGenerationEnvironment : IDomainMetadata, IServiceProviderContainer
{
    string TargetSystemName { get; }

    IReadOnlyList<Type> SecurityRuleTypeList { get; }

    IReadOnlyCollection<IProjectionEnvironment> ProjectionEnvironments { get; }

    IDomainTypeRootExtendedMetadata ExtendedMetadata { get; }

    bool IsHierarchical(Type type);
}
