using Framework.Authorization.TestGenerate.Configurations.Services.Main;

namespace Framework.Authorization.TestGenerate.Configurations.Services.WebApi;

public class MainControllerConfiguration(AuthorizationGenerationEnvironment environment) : MainServiceGeneratorConfiguration(environment)
{
    public override string Namespace { get; } = "Framework.Authorization.WebApi";

    public override string ImplementClassName { get; } = "AuthMainController";
}
