using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Base;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Base.ByProperty;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Observable;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Visual;

namespace Framework.DomainDriven.DTOGenerator.TypeScript;

/// <summary>
/// Observable file type extensions
/// </summary>
public static class ObservableFileTypeExtensions
{
    public static DTOFileType AsPlainFileType(this FileType fileType)
    {
        if (fileType == ObservableFileType.ObservableVisualDTO)
        {
            return FileType.VisualDTO;
        }

        if (fileType == ObservableFileType.ObservableProjectionDTO)
        {
            return FileType.ProjectionDTO;
        }

        if (fileType == ObservableFileType.BaseObservablePersistentDTO)
        {
            return FileType.BasePersistentDTO;
        }

        if (fileType == ObservableFileType.BaseObservableAuditPersistentDTO)
        {
            return FileType.BaseAuditPersistentDTO;
        }

        if (fileType == ObservableFileType.ObservableSimpleDTO)
        {
            return FileType.SimpleDTO;
        }

        if (fileType == ObservableFileType.ObservableFullDTO)
        {
            return FileType.FullDTO;
        }

        if (fileType == ObservableFileType.ObservableRichDTO)
        {
            return FileType.RichDTO;
        }

        if (fileType == ObservableFileType.ObservableSimpleInterfaceDTO)
        {
            return ClientFileType.SimpleInterfaceDTO;
        }

        if (fileType == ObservableFileType.ObservableFullInterfaceDTO)
        {
            return ClientFileType.FullInterfaceDTO;
        }

        if (fileType == ObservableFileType.ObservableRichInterfaceDTO)
        {
            return ClientFileType.RichInterfaceDTO;
        }

        return null;
    }

    public static DTOFileType AsObservableFileType(this FileType fileType)
    {
        if (fileType == FileType.ProjectionDTO)
        {
            return ObservableFileType.ObservableProjectionDTO;
        }

        if (fileType == FileType.VisualDTO)
        {
            return ObservableFileType.ObservableVisualDTO;
        }

        if (fileType == FileType.BaseAbstractDTO)
        {
            return ObservableFileType.BaseObservablePersistentDTO;
        }

        if (fileType == FileType.BasePersistentDTO)
        {
            return ObservableFileType.BaseObservablePersistentDTO;
        }

        if (fileType == FileType.BaseAuditPersistentDTO)
        {
            return ObservableFileType.BaseObservableAuditPersistentDTO;
        }

        if (fileType == FileType.SimpleDTO)
        {
            return ObservableFileType.ObservableSimpleDTO;
        }

        if (fileType == FileType.FullDTO)
        {
            return ObservableFileType.ObservableFullDTO;
        }

        if (fileType == FileType.RichDTO)
        {
            return ObservableFileType.ObservableRichDTO;
        }

        if (fileType == ClientFileType.SimpleInterfaceDTO)
        {
            return ObservableFileType.ObservableSimpleInterfaceDTO;
        }

        if (fileType == ClientFileType.FullInterfaceDTO)
        {
            return ObservableFileType.ObservableFullInterfaceDTO;
        }

        if (fileType == ClientFileType.RichInterfaceDTO)
        {
            return ObservableFileType.ObservableRichInterfaceDTO;
        }

        return null;
    }

    public static PropertyFileFactory<TConfiguration, MainDTOFileType> AsPlainFactory<TConfiguration, TFileType>(this ClientFileFactory<TConfiguration, TFileType> fileFactory)
            where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
            where TFileType : DTOFileType
    {
        if (fileFactory.FileType == ObservableFileType.ObservableVisualDTO)
        {
            return new DefaultVisualDTOFileFactory<TConfiguration>(fileFactory.Configuration, fileFactory.DomainType);
        }

        if (fileFactory.FileType == ObservableFileType.BaseObservablePersistentDTO)
        {
            return new DefaultBasePersistentDTOFileFactory<TConfiguration>(fileFactory.Configuration);
        }

        if (fileFactory.FileType == ObservableFileType.BaseObservableAuditPersistentDTO)
        {
            return new DefaultBaseAuditPersistentDTOFileFactory<TConfiguration>(fileFactory.Configuration);
        }

        if (fileFactory.FileType == ObservableFileType.ObservableSimpleDTO)
        {
            return new DefaultSimpleDTOFileFactory<TConfiguration>(fileFactory.Configuration, fileFactory.DomainType);
        }

        if (fileFactory.FileType == ObservableFileType.ObservableFullDTO)
        {
            return new DefaultFullDTOFileFactory<TConfiguration>(fileFactory.Configuration, fileFactory.DomainType);
        }

        if (fileFactory.FileType == ObservableFileType.ObservableRichDTO)
        {
            return new DefaultRichDTOFileFactory<TConfiguration>(fileFactory.Configuration, fileFactory.DomainType);
        }

        if (fileFactory.FileType == ObservableFileType.ObservableProjectionDTO)
        {
            return new DefaultProjectionDTOFileFactory<TConfiguration>(fileFactory.Configuration, fileFactory.DomainType);
        }

        return null;
    }

    public static PropertyFileFactory<TConfiguration, MainDTOFileType> AsObservableFactory<TConfiguration>(this IClientFileFactory<TConfiguration, MainDTOFileType> fileFactory)
            where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        if (fileFactory.FileType == FileType.BaseAbstractDTO)
        {
            return new DefaultBaseObservableAbstractDTOFileFactory<TConfiguration>(fileFactory.Configuration);
        }

        if (fileFactory.FileType == FileType.VisualDTO)
        {
            return new DefaultObservableVisualDTOFileFactory<TConfiguration>(fileFactory.Configuration, fileFactory.DomainType);
        }

        if (fileFactory.FileType == FileType.ProjectionDTO)
        {
            return new DefaultObservableProjectionDTOFileFactory<TConfiguration>(fileFactory.Configuration, fileFactory.DomainType);
        }

        if (fileFactory.FileType == FileType.BasePersistentDTO)
        {
            return new DefaultBaseObservablePersistentDTOFileFactory<TConfiguration>(fileFactory.Configuration);
        }

        if (fileFactory.FileType == FileType.BaseAuditPersistentDTO)
        {
            return new DefaultBaseObservableAuditPersistentDTOFileFactory<TConfiguration>(fileFactory.Configuration);
        }

        if (fileFactory.FileType == FileType.SimpleDTO)
        {
            return new DefaultObservableSimpleDTOFileFactory<TConfiguration>(fileFactory.Configuration, fileFactory.DomainType);
        }

        if (fileFactory.FileType == FileType.FullDTO)
        {
            return new DefaultObservableFullDTOFileFactory<TConfiguration>(fileFactory.Configuration, fileFactory.DomainType);
        }

        if (fileFactory.FileType == FileType.RichDTO)
        {
            return new DefaultObservableRichDTOFileFactory<TConfiguration>(fileFactory.Configuration, fileFactory.DomainType);
        }

        return null;
    }
}
