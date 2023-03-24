using System.Collections.Generic;

using Framework.DomainDriven.Generation;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.ServiceModelGenerator;

public class ServiceModelFileGenerator : ServiceModelFileGenerator<IGeneratorConfigurationBase<IGenerationEnvironmentBase>>
{
    public ServiceModelFileGenerator(IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration)
            : base(configuration)
    {
    }
}

public class ServiceModelFileGenerator<TConfiguration> : CodeFileGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public ServiceModelFileGenerator(TConfiguration configuration)
            : base(configuration)
    {
    }


    protected override IEnumerable<ICodeFile> GetInternalFileGenerators()
    {
        yield return new AccumImplementFileFactory<TConfiguration>(this.Configuration);

        foreach (var domainType in this.Configuration.GetActualDomainTypes())
        {
            yield return new ImplementFileFactory<TConfiguration>(this.Configuration, domainType);
        }
    }
}
