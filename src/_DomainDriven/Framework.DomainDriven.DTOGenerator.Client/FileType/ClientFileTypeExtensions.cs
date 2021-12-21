using System;

namespace Framework.DomainDriven.DTOGenerator.Client
{
    public static class ClientFileTypeExtensions
    {
        public static bool HasBaseInterfaceType(this FileType fileType)
        {
            if (fileType == null) throw new ArgumentNullException(nameof(fileType));

            return fileType == FileType.BaseAbstractDTO
                || fileType == FileType.BaseAuditPersistentDTO
                || fileType == FileType.SimpleDTO
                || fileType == FileType.FullDTO
                || fileType == FileType.RichDTO;
        }
    }
}