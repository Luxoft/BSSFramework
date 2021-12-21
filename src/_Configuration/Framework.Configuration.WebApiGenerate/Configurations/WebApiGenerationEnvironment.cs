using System;
using Framework.Configuration.TestGenerate;

namespace Framework.Configuration.WebApiGenerate
{
    public class WebApiGenerationEnvironment : ServerGenerationEnvironment
    {
        public readonly MainSLControllerConfiguration MainSLController;

        public WebApiGenerationEnvironment()
        {
            this.MainSLController = new MainSLControllerConfiguration(this);
        }
    }
}
