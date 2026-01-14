using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.BLLGenerator;

public interface IGenerationEnvironmentBase : IGenerationEnvironment,

                                              BLLCoreGenerator.IGeneratorConfigurationContainer
{
    bool GenerateFetchService { get; }
}
