using Framework.Workflow.TestGenerate;

namespace Framework.Workflow.WebApiGenerate
{
    public class MainSLControllerConfiguration : MainServiceGeneratorConfiguration
    {
        public MainSLControllerConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
        {
        }

        public override string Namespace { get; } = "Framework.Workflow.WebApi";

        public override string ImplementClassName { get; } = "WorkflowSLJsonController";
    }
}
