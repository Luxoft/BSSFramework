using Framework.Authorization.TestGenerate.Configurations;
using Framework.Authorization.TestGenerate.Configurations.Services.Main;

namespace Framework.Authorization.WebApiGenerate.Configurations;

public class MainControllerConfiguration(ServerGenerationEnvironment environment) : MainServiceGeneratorConfiguration(environment)
{
    public override string Namespace { get; } = "Framework.Authorization.WebApi";

    public override string ImplementClassName { get; } = "AuthMainController";
}
