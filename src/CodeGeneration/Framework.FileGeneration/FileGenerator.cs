using Framework.FileGeneration.Configuration;

namespace Framework.FileGeneration;

public abstract class FileGenerator<TConfiguration, TRenderingData, TRenderer>(TConfiguration configuration)
    : GeneratorConfigurationContainer<TConfiguration>(configuration), IFileGenerator<TRenderingData, TRenderer>
    where TConfiguration : class, IFileGeneratorConfiguration
{
    public abstract TRenderer Renderer { get; }

    public abstract IEnumerable<TRenderingData> GetFileGenerators();
}
