using Framework.Transfering;

using GenericQueryable.Fetching;

namespace Framework.DomainDriven;

public record DTOFetchRule<TSource>(ViewDTOType Value) : FetchRuleHeader<TSource, ViewDTOType>(Value)
{
    public DTOFetchRule(MainDTOType value)
        : this((ViewDTOType)value)
    {
    }
}
