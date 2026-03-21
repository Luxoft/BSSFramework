using Framework.BLL.Domain.Dto;

using GenericQueryable.Fetching;

namespace Framework.BLL;

public record DTOFetchRule<TSource>(ViewDTOType Value) : FetchRuleHeader<TSource, ViewDTOType>(Value)
{
    public DTOFetchRule(MainDTOType value)
        : this((ViewDTOType)value)
    {
    }
}
