using Framework.Configuration.TestGenerate.Configurations;

namespace Framework.Configuration.WebApiGenerate;

public class WebApiGenerationEnvironment : ServerGenerationEnvironment
{
    public readonly MainControllerConfiguration MainController;

    public WebApiGenerationEnvironment() => this.MainController = new MainControllerConfiguration(this);
}
