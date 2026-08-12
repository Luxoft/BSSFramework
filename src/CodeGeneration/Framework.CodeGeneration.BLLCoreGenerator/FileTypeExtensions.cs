using Anch.Core;

using Framework.CodeGeneration.FileFactory;

namespace Framework.CodeGeneration.BLLCoreGenerator;

public static class FileTypeExtensions
{
    public static ICodeFileFactoryHeader<FileType> ToHeader(this FileType fileType, string? contextTypePrefix = null) => new CodeFileFactoryHeader<FileType>(fileType, "", @domainType => contextTypePrefix + @domainType.Maybe(v => v.Name) + fileType);
}
