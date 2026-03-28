namespace Framework.Authorization.TestGenerate;

public partial class ServerGenerators(ServerGenerationEnvironment environment) : GeneratorsBase
{
    protected readonly ServerGenerationEnvironment Environment = environment ?? throw new ArgumentNullException(nameof(environment));

    public ServerGenerators()
            : this(ServerGenerationEnvironment.Default)
    {
    }

    protected override string GeneratePath => this.FrameworkPath + @"/src/_Authorization";
}
