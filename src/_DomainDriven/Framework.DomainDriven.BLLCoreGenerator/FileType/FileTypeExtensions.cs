using CommonFramework;

using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.BLLCoreGenerator;

public static class FileTypeExtensions
{
    public static ICodeFileFactoryHeader<FileType> ToHeader(this FileType fileType, string contextTypePrefix = null)
    {
        return new CodeFileFactoryHeader<FileType>(fileType, "", @domainType => contextTypePrefix + @domainType.Maybe(v => v.Name) + fileType);
    }
}
