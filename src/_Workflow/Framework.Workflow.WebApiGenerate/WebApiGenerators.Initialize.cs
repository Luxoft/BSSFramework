using Framework.Workflow.TestGenerate;

namespace Framework.Workflow.WebApiGenerate
{
    public partial class WebApiGenerators : GeneratorsBase
    {
        protected readonly WebApiGenerationEnvironment Environment = new WebApiGenerationEnvironment();

        protected override string GeneratePath => this.FrameworkPath + @"\src\_Workflow";
    }
}
