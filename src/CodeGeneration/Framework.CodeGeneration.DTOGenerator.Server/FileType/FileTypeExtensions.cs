using Framework.CodeGeneration.DTOGenerator.FileTypes;

namespace Framework.CodeGeneration.DTOGenerator.Server.FileType;

public static class FileTypeExtensions
{
    public static bool NeedMappingServiceForConvert(this BaseFileType fileType)
    {
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        return fileType != BaseFileType.IdentityDTO;
    }
}
