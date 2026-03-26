using CommonFramework;

using Framework.CodeGeneration.FileFactory;

namespace Framework.CodeGeneration.BLLCoreGenerator.FileType;

public static class FileTypeExtensions
{
    public static ICodeFileFactoryHeader<FileType> ToHeader(this FileType fileType, string? contextTypePrefix = null)
    {
        return new CodeFileFactoryHeader<FileType>(fileType, "", @domainType => contextTypePrefix + @domainType.Maybe(v => v.Name) + fileType);
    }
}
