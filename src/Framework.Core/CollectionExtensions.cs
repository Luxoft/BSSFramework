namespace Framework.Core;

public static class CollectionExtensions
{
    public static void RemoveBy<T>(this ICollection<T> source, Func<T, bool> selector)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (selector == null)
        {
            throw new ArgumentNullException(nameof(selector));
        }

        source.Where(selector).ToList().ForEach(v => source.Remove(v));
    }
}
