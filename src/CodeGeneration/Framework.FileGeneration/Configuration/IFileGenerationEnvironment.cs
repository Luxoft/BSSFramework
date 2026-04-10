using Framework.Core;
using Framework.ExtendedMetadata;
using Framework.Projection;

namespace Framework.FileGeneration.Configuration;

public interface IFileGenerationEnvironment : IDomainMetadata, IServiceProviderContainer
{
    string TargetSystemName { get; }

    IReadOnlyCollection<IProjectionEnvironment> ProjectionEnvironments { get; }

    IMetadataProxyProvider MetadataProxyProvider { get; }
}
