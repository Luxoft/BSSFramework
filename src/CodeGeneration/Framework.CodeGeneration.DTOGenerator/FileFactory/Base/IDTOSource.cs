using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.FileFactory;
using Framework.FileGeneration.Configuration;

namespace Framework.CodeGeneration.DTOGenerator.FileFactory.Base;

public interface IDTOSource : IDomainTypeContainer//, IFileTypeSource<DTOFileType>
{
    DTOFileType FileType { get; }
}

public interface IDTOSource<out TConfiguration> : IDTOSource, IFileGeneratorConfigurationContainer<TConfiguration>
        where TConfiguration : class, IDTOGeneratorConfiguration<IDTOGenerationEnvironment>;
