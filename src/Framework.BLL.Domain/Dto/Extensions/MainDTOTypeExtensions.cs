namespace Framework.BLL.Domain.DTO.Extensions;

public static class MainDTOTypeExtensions
{
    public static string WithoutPostfix(this MainDTOType dtoType) => ((DTOType)dtoType).WithoutPostfix();

    public static MainDTOType Min(this MainDTOType v1, MainDTOType v2) => v1 > v2 ? v2 : v1;

    public static MainDTOType Max(this MainDTOType v1, MainDTOType v2) => v1 < v2 ? v2 : v1;
}
