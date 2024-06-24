using Framework.Persistent;

namespace Framework.OData;

public static class SelectOperationResultExtensions
{
    public static SelectOperationResult<T> ToSelectOperationResult<T>(this IEnumerable<T> items, int totalCount)
    {
        if (items == null) throw new ArgumentNullException(nameof(items));

        return new SelectOperationResult<T>(items, totalCount);
    }


    public static SelectOperationResult<TResult> Select<TSource, TResult>(this SelectOperationResult<TSource> source, Func<TSource, TResult> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return source.Items.Select(selector).ToSelectOperationResult(source.TotalCount);
    }

    public static SelectOperationResult<HierarchicalNode<TResult, TIdent>> ChangeItem<TSource, TResult, TIdent>(this SelectOperationResult<HierarchicalNode<TSource, TIdent>> source, Func<TSource, TResult> selector)
            where TSource : IIdentityObject<TIdent>
            where TResult : IIdentityObject<TIdent>
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return source.Items.Select(node => node.ChangeItem(selector)).ToSelectOperationResult(source.TotalCount);
    }

    public static SelectOperationResult<T> Where<T>(this SelectOperationResult<T> source, Func<T, bool> filter)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        return source.Items.Where(filter).ToSelectOperationResult(source.TotalCount);
    }
}
