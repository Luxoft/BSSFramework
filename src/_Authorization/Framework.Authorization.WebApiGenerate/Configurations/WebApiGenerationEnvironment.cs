using Framework.Authorization.TestGenerate;

namespace Framework.Authorization.WebApiGenerate;

public class WebApiGenerationEnvironment : ServerGenerationEnvironment
{
    public readonly MainSLControllerConfiguration MainSLController;

    public WebApiGenerationEnvironment()
    {
        this.MainSLController = new MainSLControllerConfiguration(this);
    }
}
