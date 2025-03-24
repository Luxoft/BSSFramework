using System.Collections.ObjectModel;

using Framework.Core;

namespace Framework.DomainDriven.NHibernate;

internal static class QueryableExtensions
{
    public static IQueryable<TDomainObject> WithFetchs<TDomainObject>(
        this IQueryable<TDomainObject> queryable,
        IFetchContainer<TDomainObject> fetchContainer)
    {
        if (queryable == null) throw new ArgumentNullException(nameof(queryable));

        if (fetchContainer == null || !fetchContainer.Fetchs.Any())
        {
            return queryable;
        }
        else
        {
            var func = FetchHelper<TDomainObject>.Cache[fetchContainer //.Compress()
                                                        .Fetchs.SelectMany(fetch => fetch.ToPropertyPaths()).ToReadOnlyCollection()];

            return func(queryable);
        }
    }

    private static class FetchHelper<TDomainObject>
    {
        public static readonly
            IDictionaryCache<ReadOnlyCollection<PropertyPath>, Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>>> Cache =
                new DictionaryCache<ReadOnlyCollection<PropertyPath>, Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>>>(
                    paths =>
                    {
                        var fetchExpr = NHibFetchService<TDomainObject>.GetWithFetchExpr(paths);

                        return fetchExpr.Compile();

                    },
                    SequenceComparer<PropertyPath>.Value).WithLock();
    }
}
