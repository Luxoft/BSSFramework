using Framework.Configuration.TestGenerate.Configurations;
using Framework.Configuration.TestGenerate.Configurations.Services.Main;

namespace Framework.Configuration.WebApiGenerate;

public class MainControllerConfiguration(ServerGenerationEnvironment environment) : MainServiceGeneratorConfiguration(environment)
{
    public override string Namespace { get; } = "Framework.Configuration.WebApi";

    public override string ImplementClassName { get; } = "ConfigMainController";
}
