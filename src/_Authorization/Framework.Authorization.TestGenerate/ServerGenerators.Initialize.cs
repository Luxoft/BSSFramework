namespace Framework.Authorization.TestGenerate;

public partial class ServerGenerators : GeneratorsBase
{
    protected readonly ServerGenerationEnvironment Environment;

    public ServerGenerators()
            : this(ServerGenerationEnvironment.Default)
    {
    }

    public ServerGenerators(ServerGenerationEnvironment environment)
    {
        this.Environment = environment ?? throw new ArgumentNullException(nameof(environment));
    }

    protected override string GeneratePath => this.FrameworkPath + @"/src/_Authorization";
}
