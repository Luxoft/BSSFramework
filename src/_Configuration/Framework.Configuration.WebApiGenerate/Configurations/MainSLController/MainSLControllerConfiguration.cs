using Framework.Configuration.TestGenerate;
using Framework.Configuration.TestGenerate.Configurations;
using Framework.Configuration.TestGenerate.Configurations.Services.Main;

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
