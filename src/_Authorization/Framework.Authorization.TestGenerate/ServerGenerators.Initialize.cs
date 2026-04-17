using Framework.Authorization.TestGenerate.Configurations;

namespace Framework.Authorization.TestGenerate;

public partial class ServerGenerators(AuthorizationGenerationEnvironment? environment = null) : GeneratorsBase
{
    protected readonly AuthorizationGenerationEnvironment Environment = environment ?? AuthorizationGenerationEnvironment.Default;

    protected override string GeneratePath => this.FrameworkPath + @"/src/_Authorization";
}
