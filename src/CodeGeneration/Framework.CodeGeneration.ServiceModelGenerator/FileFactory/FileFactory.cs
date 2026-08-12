using Framework.CodeGeneration.FileFactory;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators;

namespace Framework.CodeGeneration.ServiceModelGenerator.FileFactory;

public abstract class FileFactory<TConfiguration>(TConfiguration configuration, Type domainType) : CodeFileFactory<TConfiguration, FileType>(configuration, domainType)
    where TConfiguration : class, IServiceModelGeneratorConfiguration<IServiceModelGenerationEnvironment>
{
    protected IEnumerable<IServiceMethodGenerator> GetMethodGenerators() => this.Configuration.GetActualMethodGenerators(this.DomainType!);
}
