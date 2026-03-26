using Framework.CodeGeneration.DomainMetadata;

namespace Framework.CodeGeneration.BLLCoreGenerator.Configuration;

public interface IGeneratorConfigurationContainer : IGenerationEnvironment
{
    IGeneratorConfigurationBase<IGenerationEnvironmentBase> BLLCore { get; }
}
