using Framework.Configuration.TestGenerate;

namespace Framework.Configuration.WebApiGenerate;

public class MainSLControllerConfiguration : MainServiceGeneratorConfiguration
{
    public MainSLControllerConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
    {
    }

    public override string Namespace { get; } = "Framework.Configuration.WebApi";

    public override string ImplementClassName { get; } = "ConfigSLJsonController";
}
