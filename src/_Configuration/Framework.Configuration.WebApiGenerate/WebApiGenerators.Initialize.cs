using Framework.Configuration.TestGenerate;
using Framework.Configuration.TestGenerate._Base;

namespace Framework.Configuration.WebApiGenerate;

public partial class WebApiGenerators : GeneratorsBase
{
    protected readonly WebApiGenerationEnvironment Environment = new WebApiGenerationEnvironment();

    protected override string GeneratePath => this.FrameworkPath + @"\src\_Configuration";
}
