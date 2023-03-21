using Framework.Core;

namespace Framework.Transfering;

public static class ViewDTOTypeExtensions
{
    public static string WithoutPostfix(this ViewDTOType dtoType)
    {
        return dtoType.ToString().SkipLast("DTO", true);
    }

    public static ViewDTOType Min(this ViewDTOType v1, ViewDTOType v2)
    {
        return v1 > v2 ? v2 : v1;
    }

    public static ViewDTOType Max(this ViewDTOType v1, ViewDTOType v2)
    {
        return v1 < v2 ? v2 : v1;
    }
}
