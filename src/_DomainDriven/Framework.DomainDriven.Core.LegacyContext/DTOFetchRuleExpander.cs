using Framework.Transfering;

using GenericQueryable.Fetching;

namespace Framework.DomainDriven;

public abstract class DTOFetchRuleExpander<TPersistentObjectBase> : IFetchRuleExpander
{
    public PropertyFetchRule<TSource>? TryExpand<TSource>(FetchRule<TSource> fetchRule)
    {
        if (typeof(TPersistentObjectBase).IsAssignableFrom(typeof(TSource))
            && fetchRule is DTOFetchRule<TSource> dtoFetchRule)
        {
            return this.TryExpand<TSource>(dtoFetchRule.Value);
        }

        return null;
    }

    protected abstract PropertyFetchRule<TSource>? TryExpand<TSource>(ViewDTOType dtoType);
}
