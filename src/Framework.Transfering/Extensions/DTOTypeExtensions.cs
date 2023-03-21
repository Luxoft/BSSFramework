using Framework.Core;

namespace Framework.Transfering;

public static class DTOTypeExtensions
{
    public static string WithoutPostfix(this DTOType dtoType)
    {
        return dtoType.ToString().SkipLast("DTO", true);
    }

    public static DTOType Min(this DTOType v1, DTOType v2)
    {
        return v1 > v2 ? v2 : v1;
    }

    public static DTOType Max(this DTOType v1, DTOType v2)
    {
        return v1 < v2 ? v2 : v1;
    }
}
