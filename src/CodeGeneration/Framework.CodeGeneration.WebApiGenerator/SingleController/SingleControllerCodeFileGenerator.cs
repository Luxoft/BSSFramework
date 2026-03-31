using Framework.CodeGeneration.ServiceModelGenerator;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration;
using Framework.CodeGeneration.ServiceModelGenerator.FileFactory;

namespace Framework.CodeGeneration.WebApiGenerator.SingleController;

public class SingleControllerCodeFileGenerator(IServiceModelGeneratorConfiguration<IServiceModelGenerationEnvironment> configuration)
    : SingleControllerCodeFileGenerator<IServiceModelGeneratorConfiguration<IServiceModelGenerationEnvironment>>(configuration);

public class SingleControllerCodeFileGenerator<TConfiguration>(TConfiguration configuration) : ServiceModelFileGenerator<TConfiguration>(configuration)
    where TConfiguration : class, IServiceModelGeneratorConfiguration<IServiceModelGenerationEnvironment>
{
    protected override IEnumerable<ICodeFile> GetInternalFileGenerators()
    {
        foreach (var baseFileGenerator in base.GetInternalFileGenerators())
        {
            if (baseFileGenerator is ImplementFileFactory<TConfiguration> fileFactory)
            {
                yield return new SingleControllerCodeFileFactory<TConfiguration>(this.Configuration, fileFactory.DomainType);
            }
        }
    }
}
