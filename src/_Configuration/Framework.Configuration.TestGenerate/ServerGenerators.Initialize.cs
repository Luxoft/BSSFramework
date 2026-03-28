using Framework.Configuration.TestGenerate._Base;
using Framework.Configuration.TestGenerate.Configurations;

namespace Framework.Configuration.TestGenerate;

public partial class ServerGenerators(ServerGenerationEnvironment environment) : GeneratorsBase
{
    protected readonly ServerGenerationEnvironment Environment = environment ?? throw new ArgumentNullException(nameof(environment));

    public ServerGenerators()
            : this(ServerGenerationEnvironment.Default)
    {
    }

    protected override string GeneratePath => this.FrameworkPath + @"/src/_Configuration";
}
