using Framework.CodeGeneration.ServiceModelGenerator.Configuration;
using Framework.CodeGeneration.ServiceModelGenerator.FileFactory;

namespace Framework.CodeGeneration.ServiceModelGenerator;

public class ServiceModelFileGenerator(IServiceModelGeneratorConfiguration<IServiceModelGenerationEnvironment> configuration)
    : ServiceModelFileGenerator<IServiceModelGeneratorConfiguration<IServiceModelGenerationEnvironment>>(configuration);

public class ServiceModelFileGenerator<TConfiguration>(TConfiguration configuration) : CodeFileGenerator<TConfiguration>(configuration)
    where TConfiguration : class, IServiceModelGeneratorConfiguration<IServiceModelGenerationEnvironment>
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
