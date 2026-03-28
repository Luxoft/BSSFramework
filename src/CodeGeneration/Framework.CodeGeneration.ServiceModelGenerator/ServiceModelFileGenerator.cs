using Framework.CodeGeneration.ServiceModelGenerator.Configuration._Base;
using Framework.CodeGeneration.ServiceModelGenerator.FileFactory;

namespace Framework.CodeGeneration.ServiceModelGenerator;

public class ServiceModelFileGenerator(IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration)
    : ServiceModelFileGenerator<IGeneratorConfigurationBase<IGenerationEnvironmentBase>>(configuration);

public class ServiceModelFileGenerator<TConfiguration>(TConfiguration configuration) : CodeFileGenerator<TConfiguration>(configuration)
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    protected override IEnumerable<ICodeFile> GetInternalFileGenerators()
    {
        yield return new AccumImplementFileFactory<TConfiguration>(this.Configuration);

        foreach (var domainType in this.Configuration.GetActualDomainTypes())
        {
            yield return new ImplementFileFactory<TConfiguration>(this.Configuration, domainType);
        }
    }
}
