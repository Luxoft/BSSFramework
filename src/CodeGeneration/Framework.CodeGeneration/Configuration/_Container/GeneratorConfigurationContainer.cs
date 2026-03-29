namespace Framework.CodeGeneration.Configuration._Container;

public class GeneratorConfigurationContainer<TConfiguration>(TConfiguration configuration) : IGeneratorConfigurationContainer<TConfiguration>
    where TConfiguration : class
{
    public TConfiguration Configuration { get; } = configuration;
}
