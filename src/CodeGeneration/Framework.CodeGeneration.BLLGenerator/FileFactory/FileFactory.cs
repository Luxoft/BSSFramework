using Framework.CodeGeneration.BLLGenerator.Configuration;
using Framework.CodeGeneration.FileFactory;

namespace Framework.CodeGeneration.BLLGenerator.FileFactory;

public abstract class FileFactory<TConfiguration>(TConfiguration configuration, Type? domainType) :
    CodeFileFactory<TConfiguration, FileType>(configuration, domainType)
    where TConfiguration : class, IBLLGeneratorConfiguration<IBLLGenerationEnvironment>;
