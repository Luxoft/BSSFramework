using Framework.Authorization.TestGenerate;

namespace Framework.Authorization.WebApiGenerate;

public class MainControllerConfiguration(ServerGenerationEnvironment environment) : MainServiceGeneratorConfiguration(environment)
{
    public override string Namespace { get; } = "Framework.Authorization.WebApi";

    public override string ImplementClassName { get; } = "AuthSLJsonController";
}
