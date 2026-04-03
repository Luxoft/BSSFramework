using Framework.CodeGeneration.ServiceModelGenerator.Configuration.Main;

namespace Framework.Configuration.TestGenerate.Configurations.Services.Main;

public class MainServiceGeneratorConfiguration(ConfigurationGenerationEnvironment environment) : MainGeneratorConfigurationBase<ConfigurationGenerationEnvironment>(environment)
{
    public override string ImplementClassName { get; } = "ConfigurationFacade";
}
