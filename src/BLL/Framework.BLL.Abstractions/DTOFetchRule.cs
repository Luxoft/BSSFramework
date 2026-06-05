using Anch.GenericQueryable.Fetching;

using Framework.BLL.Domain.DTO;

namespace Framework.BLL;

public record DTOFetchRule<TSource>(ViewDTOType Value) : FetchRuleHeader<TSource, ViewDTOType>(Value)
{
    public DTOFetchRule(MainDTOType value)
        : this((ViewDTOType)value)
    {
    }
}

