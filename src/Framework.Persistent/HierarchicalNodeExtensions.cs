namespace Framework.Persistent;

public static class HierarchicalNodeExtensions
{
    public static List<HierarchicalNode<TResult, TIdent>> ChangeItem<TSource, TResult, TIdent>(this IEnumerable<HierarchicalNode<TSource, TIdent>> source, Func<TSource, TResult> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return source.Select(v => v.ChangeItem(selector)).ToList();
    }
}
