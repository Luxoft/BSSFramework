using Framework.DomainDriven.Generation;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.BLLGenerator;

public class BLLFactoryContainerGeneratorConfiguration<TConfiguration>(TConfiguration configuration)
    : GeneratorConfigurationContainer<TConfiguration>(configuration), IBLLFactoryContainerGeneratorConfiguration
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public IEnumerable<ICodeFile> GetFileFactories()
    {
        foreach (var domainType in this.Configuration.DomainTypes)
        {
            yield return new BLLFileFactory<TConfiguration>(this.Configuration, domainType);

            yield return new BLLFactoryFileFactory<TConfiguration>(this.Configuration, domainType);
        }

        yield return new BLLFactoryContainerFileFactory<TConfiguration>(this.Configuration);
    }

}
