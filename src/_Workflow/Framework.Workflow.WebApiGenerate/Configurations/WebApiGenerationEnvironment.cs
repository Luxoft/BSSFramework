using System;
using Framework.Workflow.TestGenerate;

namespace Framework.Workflow.WebApiGenerate
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
