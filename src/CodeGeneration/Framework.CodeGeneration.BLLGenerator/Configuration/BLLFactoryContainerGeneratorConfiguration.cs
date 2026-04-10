using Framework.CodeGeneration.BLLGenerator.FileFactory;
using Framework.FileGeneration.Configuration;

namespace Framework.CodeGeneration.BLLGenerator.Configuration;

public class BLLFactoryContainerGeneratorConfiguration<TConfiguration>(TConfiguration configuration)
    : GeneratorConfigurationContainer<TConfiguration>(configuration), IBLLFactoryContainerGeneratorConfiguration
    where TConfiguration : class, IBLLGeneratorConfiguration<IBLLGenerationEnvironment>
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
