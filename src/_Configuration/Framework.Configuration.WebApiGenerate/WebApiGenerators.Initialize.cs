using Framework.Configuration.TestGenerate;

namespace Framework.Configuration.WebApiGenerate
{
    public partial class WebApiGenerators : GeneratorsBase
    {
        protected readonly WebApiGenerationEnvironment Environment = new WebApiGenerationEnvironment();

        protected override string GeneratePath => this.FrameworkPath + @"\src\_Configuration";
    }
}
