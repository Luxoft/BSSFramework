using Framework.Transfering;

namespace Framework.DomainDriven;

public static class DTOFetchRuleExtensions
{
    public static DTOFetchRule<TSource> ToFetchRule<TSource>(this ViewDTOType value) => new(value);

    public static DTOFetchRule<TSource> ToFetchRule<TSource>(this MainDTOType value) => ((ViewDTOType)value).ToFetchRule<TSource>();
}
