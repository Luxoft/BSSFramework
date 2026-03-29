namespace Framework.CodeGeneration.Configuration._Container;

public interface IGeneratorConfigurationContainer<out TConfiguration>
{
    TConfiguration Configuration { get; }
}
