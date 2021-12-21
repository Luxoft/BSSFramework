using Framework.Authorization.TestGenerate;

namespace Framework.Authorization.WebApiGenerate
{
    public partial class WebApiGenerators : GeneratorsBase
    {
        protected readonly WebApiGenerationEnvironment Environment = new WebApiGenerationEnvironment();

        protected override string GeneratePath => this.FrameworkPath + @"\src\_Authorization";
    }
}
