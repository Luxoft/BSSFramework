namespace Framework.BLL.Domain.Persistent;

public static class HierarchicalNodeExtensions
{
    public static List<HierarchicalNode<TResult, TIdent>> ChangeItem<TSource, TResult, TIdent>(this IEnumerable<HierarchicalNode<TSource, TIdent>> source, Func<TSource, TResult> selector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (selector is null) throw new ArgumentNullException(nameof(selector));

        return source.Select(v => v.ChangeItem(selector)).ToList();
    }
}
