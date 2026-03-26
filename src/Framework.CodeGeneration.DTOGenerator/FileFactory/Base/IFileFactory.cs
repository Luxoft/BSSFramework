using Framework.CodeGeneration.Configuration._Container;
using Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService.Base;
using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileType;
using Framework.CodeGeneration.FileFactory;

namespace Framework.CodeGeneration.DTOGenerator.FileFactory.Base;

public interface IFileFactory<out TConfiguration, out TFileType> : IFileFactory<TConfiguration>, ICodeFileFactory<TFileType>
        where TConfiguration : IGeneratorConfigurationBase<IGenerationEnvironmentBase>
        where TFileType : BaseFileType
{
}

public interface IFileFactory<out TConfiguration> : IFileFactory, IGeneratorConfigurationContainer<TConfiguration>
        where TConfiguration : IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
}

public interface IFileFactory : ICodeFileFactory
{
    string FileTypeName { get; }

    IPropertyCodeTypeReferenceService CodeTypeReferenceService { get; }
}
