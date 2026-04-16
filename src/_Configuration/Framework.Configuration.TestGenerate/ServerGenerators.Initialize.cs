using Framework.Configuration.TestGenerate.Configurations;

namespace Framework.Configuration.TestGenerate;

public partial class ServerGenerators(ConfigurationGenerationEnvironment? environment = null) : GeneratorsBase
{
    protected readonly ConfigurationGenerationEnvironment Environment = environment ?? ConfigurationGenerationEnvironment.Default;

    protected override string GeneratePath => this.FrameworkPath + @"/src/_Configuration";
}
