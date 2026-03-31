namespace Framework.FileGeneration.Configuration;

public class GeneratorConfigurationContainer<TConfiguration>(TConfiguration configuration) : IFileGeneratorConfigurationContainer<TConfiguration>
    where TConfiguration : class
{
    public TConfiguration Configuration { get; } = configuration;
}
