using System;

using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator;

public interface IFileFactory<out TConfiguration, out TFileType> : IFileFactory<TConfiguration>, ICodeFileFactory<TFileType>
        where TConfiguration : IGeneratorConfigurationBase<IGenerationEnvironmentBase>
        where TFileType : FileType
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
