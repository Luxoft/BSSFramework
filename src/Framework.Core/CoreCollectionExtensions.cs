namespace Framework.Core;

public static class CoreCollectionExtensions
{
    public static void RemoveBy<T>(this ICollection<T> source, Func<T, bool> selector)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (selector is null)
        {
            throw new ArgumentNullException(nameof(selector));
        }

        source.Where(selector).ToList().ForEach(v => source.Remove(v));
    }
}
