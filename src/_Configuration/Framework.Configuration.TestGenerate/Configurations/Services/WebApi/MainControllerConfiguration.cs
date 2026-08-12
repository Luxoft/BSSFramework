using Framework.Configuration.TestGenerate.Configurations.Services.Main;

namespace Framework.Configuration.TestGenerate.Configurations.Services.WebApi;

public class MainControllerConfiguration(ConfigurationGenerationEnvironment environment) : MainServiceGeneratorConfiguration(environment)
{
    public override string Namespace { get; } = "Framework.Configuration.WebApi";

    public override string ImplementClassName { get; } = "ConfigMainController";
}
