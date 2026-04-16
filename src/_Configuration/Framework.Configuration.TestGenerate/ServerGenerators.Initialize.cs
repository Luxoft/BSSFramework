using Framework.Configuration.TestGenerate.Configurations;

namespace Framework.Configuration.TestGenerate;

public partial class ServerGenerators : GeneratorsBase
{
    internal ServerGenerators(ConfigurationGenerationEnvironment environment)
    {
        this.Environment = environment ?? throw new ArgumentNullException(nameof(environment));
    }

    protected readonly ConfigurationGenerationEnvironment Environment;

    public ServerGenerators()
            : this(ConfigurationGenerationEnvironment.Default)
    {
    }

    protected override string GeneratePath => this.FrameworkPath + @"/src/_Configuration";
}
