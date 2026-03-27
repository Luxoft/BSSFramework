using Framework.CodeGeneration.FileFactory;
using Framework.CodeGeneration.WebApiGenerator.Configuration._Base;
using Framework.CodeGeneration.WebApiGenerator.MethodGenerators._Base;

namespace Framework.CodeGeneration.WebApiGenerator.FileFactory._Base;

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
