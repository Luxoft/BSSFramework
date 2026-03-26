using Framework.CodeGeneration.Configuration._Container;
using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileType;
using Framework.CodeGeneration.FileFactory;

namespace Framework.CodeGeneration.DTOGenerator.FileFactory.Base;

public interface IDTOSource : IDomainTypeContainer//, IFileTypeSource<DTOFileType>
{
    DTOFileType FileType { get; }
}

public interface IDTOSource<out TConfiguration> : IDTOSource, IGeneratorConfigurationContainer<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{

}
