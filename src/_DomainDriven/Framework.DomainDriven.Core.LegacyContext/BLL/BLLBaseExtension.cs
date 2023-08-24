using System.Linq.Expressions;

namespace Framework.DomainDriven.BLL;

public static class BLLBaseExtensions
{
    public static IList<TDomainObject> GetFullList<TDomainObject, TBuildRule>(this IBLLBase<IFetchServiceContainer<TDomainObject, TBuildRule>, TDomainObject> bll, TBuildRule rule)
    {
        if (bll == null) throw new ArgumentNullException(nameof(bll));

        return bll.GetFullList(bll.Context.FetchService.GetContainer<TDomainObject>(rule));
    }

    public static IList<TDomainObject> GetObjectsBy<TDomainObject, TBuildRule>(this IBLLBase<IFetchServiceContainer<TDomainObject, TBuildRule>, TDomainObject> bll, IDomainObjectFilterModel<TDomainObject> filter, TBuildRule rule)
    {
        if (bll == null) throw new ArgumentNullException(nameof(bll));
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        return bll.GetListBy(filter, bll.Context.FetchService.GetContainer<TDomainObject>(rule));

    }

    public static IList<TDomainObject> GetObjectsBy<TDomainObject, TBuildRule>(this IBLLBase<IFetchServiceContainer<TDomainObject, TBuildRule>, TDomainObject> bll, Expression<Func<TDomainObject, bool>> filter, TBuildRule rule)
    {
        if (bll == null) throw new ArgumentNullException(nameof(bll));
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        return bll.GetListBy(filter, bll.Context.FetchService.GetContainer<TDomainObject>(rule));
    }
}
