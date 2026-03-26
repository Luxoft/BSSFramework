using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.ServiceModelGenerator;

public abstract class FileFactory<TConfiguration> : CodeFileFactory<TConfiguration, FileType>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    protected FileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {

    }


    protected IEnumerable<IServiceMethodGenerator> GetMethodGenerators()
    {
        return this.Configuration.GetActualMethodGenerators(this.DomainType);
    }
}
