using System.Linq.Expressions;

namespace Framework.GenericQueryable;

public static class GenericQueryableExtensions
{
    public static Task<List<TSource>> ToGenericListAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.ToGenericAsync(() => source.ToGenericListAsync(cancellationToken));

    public static Task<TSource> GenericSingleAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.ToGenericAsync(() => source.GenericSingleAsync(cancellationToken));

    public static Task<TSource> GenericSingleAsync<TSource>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, bool>> filter,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.ToGenericAsync(() => source.GenericSingleAsync(filter, cancellationToken));

    public static Task<TSource?> GenericSingleOrDefaultAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.ToGenericAsync(() => source.GenericSingleOrDefaultAsync(cancellationToken));

    public static Task<TSource> GenericFirstAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.ToGenericAsync(() => source.GenericFirstAsync(cancellationToken));

    public static Task<TSource?> GenericFirstOrDefaultAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.ToGenericAsync(() => source.GenericFirstOrDefaultAsync(cancellationToken));

    public static Task<int> GenericCountAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.ToGenericAsync(() => source.GenericCountAsync(cancellationToken));

    public static Task<bool> GenericAnyAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken cancellationToken = default(CancellationToken)) =>
        source.ToGenericAsync(() => source.GenericAnyAsync(cancellationToken));

    private static Task<TResult> ToGenericAsync<TSource, TResult>(
        this IQueryable<TSource> source,
        Expression<Func<Task<TResult>>> callExpression) =>
        source.Provider.Execute<Task<TResult>>(new GenericQueryableMethodExpression(callExpression));
}
