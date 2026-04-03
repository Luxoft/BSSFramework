using Framework.CodeGeneration.BLLCoreGenerator.Configuration;
using Framework.CodeGeneration.BLLCoreGenerator.FileFactory;

namespace Framework.CodeGeneration.BLLCoreGenerator;

/// <inheritdoc />
public class BLLCoreFileGenerator(IBLLCoreGeneratorConfiguration<IBLLCoreGenerationEnvironment> configuration)
    : BLLCoreFileGenerator<IBLLCoreGeneratorConfiguration<IBLLCoreGenerationEnvironment>>(configuration);

/// <inheritdoc />
public class BLLCoreFileGenerator<TConfiguration>(TConfiguration configuration) : CodeFileGenerator<TConfiguration>(configuration)
    where TConfiguration : class, IBLLCoreGeneratorConfiguration<IBLLCoreGenerationEnvironment>
{
    protected override IEnumerable<ICodeFile> GetInternalFileGenerators()
    {
        yield return new BLLContextInterfaceFileFactory<TConfiguration>(this.Configuration);

        yield return new BLLFactoryContainerInterfaceFileFactory<TConfiguration>(this.Configuration);

        foreach (var domainType in this.Configuration.BLLDomainTypes)
        {
            yield return new BLLInterfaceFileFactory<TConfiguration>(this.Configuration, domainType);
            yield return new BLLFactoryInterfaceFileFactory<TConfiguration>(this.Configuration, domainType);
        }
    }
}
