using Framework.CodeGeneration.ServiceModelGenerator.Configuration.Main;

namespace Framework.Configuration.TestGenerate.Configurations.Services.Main;

public class MainServiceGeneratorConfiguration(ServerGenerationEnvironment environment) : MainGeneratorConfigurationBase<ServerGenerationEnvironment>(environment)
{
    public override string ImplementClassName { get; } = "ConfigurationFacade";
}
