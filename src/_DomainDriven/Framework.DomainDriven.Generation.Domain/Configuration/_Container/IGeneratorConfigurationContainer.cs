namespace Framework.DomainDriven.Generation.Domain;

public interface IGeneratorConfigurationContainer<out TConfiguration>
{
    TConfiguration Configuration { get; }
}
