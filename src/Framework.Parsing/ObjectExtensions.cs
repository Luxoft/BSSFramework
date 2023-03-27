namespace Framework.Parsing;

internal static class ObjectExtensions
{
    public static IEnumerable<T> Cons<T>(this T head, IEnumerable<T> tail)
    {
        if (tail == null) throw new ArgumentNullException(nameof(tail));

        yield return head;

        foreach (var item in tail)
        {
            yield return item;
        }
    }
}
