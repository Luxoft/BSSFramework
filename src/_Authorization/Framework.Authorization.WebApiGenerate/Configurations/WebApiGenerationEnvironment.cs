using Framework.Authorization.TestGenerate;

namespace Framework.Authorization.WebApiGenerate.Configurations;

public class WebApiGenerationEnvironment : ServerGenerationEnvironment
{
    public readonly MainControllerConfiguration MainController;

    public WebApiGenerationEnvironment()
    {
        this.MainController = new MainControllerConfiguration(this);
    }
}
