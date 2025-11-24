using GenericQueryable.Services;

using NHibernate.Linq;

namespace Framework.DomainDriven.NHibernate;

public class NhibTargetMethodExtractor() : TargetMethodExtractor([typeof(LinqExtensionMethods), typeof(NhibLinqExtensions)])
{
    private static class NhibLinqExtensions
    {
        public static async Task<TSource[]> ToArrayAsync<TSource>(
            IQueryable<TSource> source,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var elements = await source.ToFuture().GetEnumerableAsync(cancellationToken);

            return elements.ToArray();
        }

        public static async Task<HashSet<TSource>> ToHashSetAsync<TSource>(
            IQueryable<TSource> source,
            IEqualityComparer<TSource>? comparer,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var elements = await source.ToFuture().GetEnumerableAsync(cancellationToken);

            return elements.ToHashSet(comparer);
        }

        public static async Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(
            IQueryable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            IEqualityComparer<TKey>? comparer,
            CancellationToken cancellationToken = default(CancellationToken))
            where TKey : notnull
        {
            var elements = await source.ToFuture().GetEnumerableAsync(cancellationToken);

            return elements.ToDictionary(keySelector, elementSelector, comparer);
        }
    }
}
