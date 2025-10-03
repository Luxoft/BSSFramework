using System.CodeDom;

using CommonFramework;

namespace Framework.DomainDriven.DTOGenerator;

public static class DTOFileTypeExtensions
{
    public static MainDTOFileType GetBaseType(this MainDTOFileType fileType, bool exceptAbstract = true)
    {
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        return fileType.BaseType.Maybe(baseType => exceptAbstract && baseType.IsAbstract ? null : baseType);
    }


    public static bool HasBaseType(this MainDTOFileType fileType, bool exceptAbstract = true)
    {
        return fileType.GetBaseType(exceptAbstract) != null;
    }

    public static IEnumerable<MainDTOFileType> GetBaseTypes(this MainDTOFileType fileType, bool exceptAbstract = true)
    {
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        return fileType.GetAllElements(ft => ft.GetBaseType(exceptAbstract), true);
    }



    public static MemberAttributes ToMapToDomainObjectMemberAttributes(this MainDTOFileType fileType)
    {
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        return fileType.GetBaseType() == null ? MemberAttributes.Public
                       : (MemberAttributes.Public | MemberAttributes.Override);
    }


    public static IEnumerable<MainDTOFileType> GetNestedTypes(this MainDTOFileType fileType)
    {
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        return fileType.GetAllElements(ft => ft.NestedType, true);
    }
}
