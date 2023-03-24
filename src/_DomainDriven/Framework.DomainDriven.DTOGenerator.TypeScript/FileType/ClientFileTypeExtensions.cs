using System;

namespace Framework.DomainDriven.DTOGenerator.TypeScript;

/// <summary>
/// Client file type extensions
/// </summary>
public static class ClientFileTypeExtensions
{
    public static bool HasBaseInterfaceType(this FileType fileType)
    {
        if (fileType == null)
        {
            throw new ArgumentNullException(nameof(fileType));
        }

        return fileType == FileType.BaseAbstractDTO
               || fileType == FileType.BasePersistentDTO
               || fileType == FileType.BaseAuditPersistentDTO
               || fileType == FileType.SimpleDTO
               || fileType == FileType.FullDTO
               || fileType == FileType.RichDTO;
    }
}
