using System;

namespace Framework.DomainDriven.DTOGenerator.Server;

public static class FileTypeExtensions
{
    public static bool NeedMappingServiceForConvert(this FileType fileType)
    {
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        return fileType != FileType.IdentityDTO;
    }
}
