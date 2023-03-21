using Framework.Authorization.TestGenerate;

namespace Framework.Authorization.WebApiGenerate;

public class MainSLControllerConfiguration : MainServiceGeneratorConfiguration
{
    public MainSLControllerConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
    {
    }

    public override string Namespace { get; } = "Framework.Authorization.WebApi";

    public override string ImplementClassName { get; } = "AuthSLJsonController";
}
