using Framework.CodeGeneration.BLLGenerator.Configuration;
using Framework.CodeGeneration.FileFactory;

namespace Framework.CodeGeneration.BLLGenerator.FileFactory.__Base;

public abstract class FileFactory<TConfiguration>(TConfiguration configuration, Type? domainType) :
    CodeFileFactory<TConfiguration, FileType>(configuration, domainType)
    where TConfiguration : class, IbllGeneratorConfiguration<IbllGenerationEnvironment>;
