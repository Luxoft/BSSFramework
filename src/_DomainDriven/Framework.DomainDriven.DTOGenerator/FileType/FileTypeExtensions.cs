using Framework.Core;
using Framework.DomainDriven.Generation.Domain;

using CommonFramework;

namespace Framework.DomainDriven.DTOGenerator;

public static class FileTypeExtensions
{
    public static ICodeFileFactoryHeader<TFileType> ToHeader<TFileType>(this TFileType fileType)
            where TFileType : FileType
    {
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        if (fileType == FileType.ProjectionDTO)
        {
            return new CodeFileFactoryHeader<TFileType>(fileType, fileType + @"\", domainType => domainType.Name.SkipLast("Projection", false) + fileType);
        }
        else
        {
            return new CodeFileFactoryHeader<TFileType>(fileType, fileType + @"\", domainType => domainType.Name + fileType);
        }
    }

    public static ICodeFileFactoryHeader<TFileType> ToHeader<TFileType>(this TFileType fileType, string relativePath, Func<Type, string> getTypeNameFunc)
            where TFileType : FileType
    {
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        return new CodeFileFactoryHeader<TFileType>(fileType, relativePath, getTypeNameFunc);
    }
}
