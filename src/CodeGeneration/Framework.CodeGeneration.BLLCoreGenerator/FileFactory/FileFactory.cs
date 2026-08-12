using Framework.CodeGeneration.BLLCoreGenerator.Configuration;
using Framework.CodeGeneration.FileFactory;

namespace Framework.CodeGeneration.BLLCoreGenerator.FileFactory;

public abstract class FileFactory<TConfiguration>(TConfiguration configuration, Type? domainType) : CodeFileFactory<TConfiguration, FileType>(configuration, domainType)
    where TConfiguration : class, IBLLCoreGeneratorConfiguration<IBLLCoreGenerationEnvironment>;
