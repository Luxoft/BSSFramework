using Framework.Configuration.TestGenerate._Base;
using Framework.Configuration.TestGenerate.Configurations;

namespace Framework.Configuration.TestGenerate;

public partial class ServerGenerators(ConfigurationGenerationEnvironment environment) : GeneratorsBase
{
    protected readonly ConfigurationGenerationEnvironment Environment = environment ?? throw new ArgumentNullException(nameof(environment));

    public ServerGenerators()
            : this(ConfigurationGenerationEnvironment.Default)
    {
    }

    protected override string GeneratePath => this.FrameworkPath + @"/src/_Configuration";
}
