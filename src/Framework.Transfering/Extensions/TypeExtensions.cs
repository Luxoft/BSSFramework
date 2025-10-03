using CommonFramework;
using CommonFramework.Maybe;

using Framework.Core;

namespace Framework.Transfering;

public static class TypeExtensions
{
    public static string GetPluralizedDomainName(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.GetDomainName().ToPluralize();
    }

    public static string GetDomainName(this Type type, bool splitByCase = false)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        var dtoName = type.Name;

        var nameRequest = from dtoType in EnumHelper.GetValues<DTOType>()

                          select dtoName.SkipLastMaybe(dtoType.ToString());


        return (nameRequest.CollectMaybe().SingleOrDefault() ?? dtoName)
                .Pipe(str => splitByCase ? str.JoinSplitByCase() : str);
    }


    public static bool IsDTOType(this Type type, DTOType? filterDtoType = null)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return type.GetDTOType().Where(dtoType => filterDtoType == null || dtoType == filterDtoType).HasValue;
    }

    public static Maybe<DTOType> GetDTOType(this Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        var request = from dtoType in EnumHelper.GetValues<DTOType>()

                      where type.Name.EndsWith(dtoType.ToString())

                      select dtoType;

        return request.SingleMaybe();
    }
}
