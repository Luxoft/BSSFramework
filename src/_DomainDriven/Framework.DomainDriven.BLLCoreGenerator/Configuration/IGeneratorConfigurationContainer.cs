using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.BLLCoreGenerator;

public interface IGeneratorConfigurationContainer : IGenerationEnvironment
{
    IGeneratorConfigurationBase<IGenerationEnvironmentBase> BLLCore { get; }
}
