using System.Collections.Concurrent;

using CommonFramework;

using Framework.Core;

using GenericQueryable.Fetching;

namespace Framework.DomainDriven.NHibernate;

public class UntypedFetchExpander : IFetchRuleExpander
{
    private readonly ConcurrentDictionary<Type, object> cache = new();

    public FetchRule<TSource>? TryExpand<TSource>(FetchRule<TSource> fetchRule)
    {
        if (fetchRule is UntypedFetchRule<TSource> untypedFetchRule)
        {
            return this.cache.GetOrAdd(typeof(TSource), _ => new ConcurrentDictionary<UntypedFetchRule<TSource>, PropertyFetchRule<TSource>>())
                       .Pipe(innerCache => (ConcurrentDictionary<UntypedFetchRule<TSource>, PropertyFetchRule<TSource>>)innerCache)
                       .Pipe(innerCache => innerCache.GetOrAdd(
                                 untypedFetchRule,
                                 _ =>
                                 {
                                     var fetchPath = new FetchPath(
                                         PropertyPath.Create(typeof(TSource), untypedFetchRule.Path.Split('.'))
                                                     .Select(prop => prop.ToGetLambdaExpression()).ToList());

                                     return new PropertyFetchRule<TSource>([fetchPath]);
                                 }));
        }

        return null;
    }
}
