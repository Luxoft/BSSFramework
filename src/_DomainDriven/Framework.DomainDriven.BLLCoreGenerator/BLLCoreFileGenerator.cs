using Framework.DomainDriven.Generation;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.BLLCoreGenerator;

/// <inheritdoc />
public class BLLCoreFileGenerator(IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration)
    : BLLCoreFileGenerator<IGeneratorConfigurationBase<IGenerationEnvironmentBase>>(configuration);

/// <inheritdoc />
public class BLLCoreFileGenerator<TConfiguration>(TConfiguration configuration) : CodeFileGenerator<TConfiguration>(configuration)
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
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
