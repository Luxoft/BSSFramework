using Framework.Authorization.TestGenerate.Configurations;

namespace Framework.Authorization.TestGenerate;

public partial class ServerGenerators(AuthorizationGenerationEnvironment environment) : GeneratorsBase
{
    protected readonly AuthorizationGenerationEnvironment Environment = environment;

    public ServerGenerators()
            : this(AuthorizationGenerationEnvironment.Default)
    {
    }

    protected override string GeneratePath => this.FrameworkPath + @"/src/_Authorization";
}
