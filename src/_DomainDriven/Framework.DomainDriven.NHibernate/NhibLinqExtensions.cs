using NHibernate.Linq;

namespace Framework.DomainDriven.NHibernate;

public static class NhibLinqExtensions
{
    extension<TSource>(IQueryable<TSource> source)
    {
        public async Task<HashSet<TSource>> ToHashSetAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await source.ToHashSetAsync(EqualityComparer<TSource>.Default, cancellationToken);
        }

        public async Task<HashSet<TSource>> ToHashSetAsync(
            IEqualityComparer<TSource>? comparer,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var list = await source.ToListAsync(cancellationToken);

            return list.ToHashSet(comparer);
        }

        public async Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TKey>(
            Func<TSource, TKey> keySelector,
            CancellationToken cancellationToken = default(CancellationToken))
            where TKey : notnull
        {
            return await source.ToDictionaryAsync(keySelector, EqualityComparer<TKey>.Default, cancellationToken);
        }


        public async Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TKey>(
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> comparer,
            CancellationToken cancellationToken = default(CancellationToken))
            where TKey : notnull
        {
            var list = await source.ToListAsync(cancellationToken);

            return list.ToDictionary(keySelector, comparer);
        }

        public async Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TKey, TElement>(
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            CancellationToken cancellationToken = default(CancellationToken))
            where TKey : notnull
        {
            return await source.ToDictionaryAsync(keySelector, elementSelector, EqualityComparer<TKey>.Default, cancellationToken);
        }

        public async Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TKey, TElement>(
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            IEqualityComparer<TKey>? comparer,
            CancellationToken cancellationToken = default(CancellationToken))
            where TKey : notnull
        {
            var list = await source.ToListAsync(cancellationToken);

            return list.ToDictionary(keySelector, elementSelector, comparer);
        }
    }
}
