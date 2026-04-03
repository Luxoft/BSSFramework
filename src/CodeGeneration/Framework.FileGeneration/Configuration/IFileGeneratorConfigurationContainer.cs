namespace Framework.FileGeneration.Configuration;

public interface IFileGeneratorConfigurationContainer<out TConfiguration>
{
    TConfiguration Configuration { get; }
}
