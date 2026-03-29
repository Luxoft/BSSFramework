using Framework.CodeGeneration.ServiceModelGenerator;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration._Base;
using Framework.CodeGeneration.ServiceModelGenerator.FileFactory;

namespace Framework.CodeGeneration.WebApiGenerator.SingleController;

public class SingleControllerCodeFileGenerator(IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration)
    : SingleControllerCodeFileGenerator<IGeneratorConfigurationBase<IGenerationEnvironmentBase>>(configuration);

public class SingleControllerCodeFileGenerator<TConfiguration>(TConfiguration configuration) : ServiceModelFileGenerator<TConfiguration>(configuration)
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
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
