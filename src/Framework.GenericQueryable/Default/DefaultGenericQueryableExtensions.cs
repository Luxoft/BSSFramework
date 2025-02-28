namespace Framework.GenericQueryable.Default;

public static class DefaultGenericQueryableExtensions
{
    public static IQueryable<T> AsDefaultGenericQueryable<T>(this IEnumerable<T> items) => new DefaultGenericQueryable<T>(items.AsQueryable());
}
