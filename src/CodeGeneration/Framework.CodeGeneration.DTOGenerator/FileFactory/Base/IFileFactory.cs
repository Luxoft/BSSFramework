using Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService.Base;
using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.FileFactory;
using Framework.FileGeneration.Configuration;

namespace Framework.CodeGeneration.DTOGenerator.FileFactory.Base;

public interface IFileFactory<out TConfiguration, out TFileType> : IFileFactory<TConfiguration>, ICodeFileFactory<TFileType>
        where TConfiguration : IDTOGeneratorConfiguration<IDTOGenerationEnvironment>
        where TFileType : BaseFileType;

public interface IFileFactory<out TConfiguration> : IFileFactory, IFileGeneratorConfigurationContainer<TConfiguration>
        where TConfiguration : IDTOGeneratorConfiguration<IDTOGenerationEnvironment>;

public interface IFileFactory : ICodeFileFactory
{
    string FileTypeName { get; }

    IPropertyCodeTypeReferenceService CodeTypeReferenceService { get; }
}
