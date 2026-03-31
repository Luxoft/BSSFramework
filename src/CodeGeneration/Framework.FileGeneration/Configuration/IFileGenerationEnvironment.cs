using Framework.Core;
using Framework.Projection;
using Framework.Projection.ExtendedMetadata;

using SecuritySystem;

namespace Framework.FileGeneration.Configuration;

public interface IFileGenerationEnvironment : IDomainMetadata, IServiceProviderContainer
{
    string TargetSystemName { get; }

    IReadOnlyCollection<IProjectionEnvironment> ProjectionEnvironments { get; }

    IDomainTypeRootExtendedMetadata ExtendedMetadata { get; }
}
