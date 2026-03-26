using CommonFramework;

using Framework.CodeGeneration.FileFactory;

namespace Framework.CodeGeneration.DTOGenerator.FileType;

public static class FileTypeExtensions
{
    public static ICodeFileFactoryHeader<TFileType> ToHeader<TFileType>(this TFileType fileType)
            where TFileType : BaseFileType
    {
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        if (fileType == BaseFileType.ProjectionDTO)
        {
            return new CodeFileFactoryHeader<TFileType>(fileType, fileType + @"\", domainType => domainType.Name.SkipLast("Projection", false) + fileType);
        }
        else
        {
            return new CodeFileFactoryHeader<TFileType>(fileType, fileType + @"\", domainType => domainType.Name + fileType);
        }
    }

    public static ICodeFileFactoryHeader<TFileType> ToHeader<TFileType>(this TFileType fileType, string relativePath, Func<Type, string> getTypeNameFunc)
            where TFileType : BaseFileType
    {
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        return new CodeFileFactoryHeader<TFileType>(fileType, relativePath, getTypeNameFunc);
    }
}
