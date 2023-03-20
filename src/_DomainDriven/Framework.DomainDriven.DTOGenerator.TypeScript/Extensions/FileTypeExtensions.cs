using System;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.Extensions;

public static class FileTypeExtensions
{
    public static bool IsStrict(this RoleFileType fileType)
    {
        return fileType == FileType.StrictDTO;
    }

    public static bool IsIdentity(this RoleFileType fileType)
    {
        return fileType == FileType.IdentityDTO;
    }
}
