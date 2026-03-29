using Framework.Configuration.TestGenerate;
using Framework.Configuration.TestGenerate.Configurations;

namespace Framework.Configuration.WebApiGenerate;

public class WebApiGenerationEnvironment : ServerGenerationEnvironment
{
    public readonly MainSLControllerConfiguration MainSLController;

    public WebApiGenerationEnvironment()
    {
        this.MainSLController = new MainSLControllerConfiguration(this);
    }
}
