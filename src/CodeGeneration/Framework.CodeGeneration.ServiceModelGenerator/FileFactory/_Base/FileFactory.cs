using Framework.CodeGeneration.FileFactory;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration._Base;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators._Base;

namespace Framework.CodeGeneration.ServiceModelGenerator.FileFactory._Base;

public abstract class FileFactory<TConfiguration>(TConfiguration configuration, Type domainType) : CodeFileFactory<TConfiguration, FileType>(configuration, domainType)
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    protected IEnumerable<IServiceMethodGenerator> GetMethodGenerators() => this.Configuration.GetActualMethodGenerators(this.DomainType!);
}
