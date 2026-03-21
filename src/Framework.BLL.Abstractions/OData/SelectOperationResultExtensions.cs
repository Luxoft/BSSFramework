using Framework.Application.Domain;
using Framework.BLL.Domain.Persistent;
using Framework.OData;

namespace Framework.BLL.OData;

public static class SelectOperationResultExtensions
{
    public static SelectOperationResult<HierarchicalNode<TResult, TIdent>> ChangeItem<TSource, TResult, TIdent>(
        this SelectOperationResult<HierarchicalNode<TSource, TIdent>> source,
        Func<TSource, TResult> selector)
        where TSource : IIdentityObject<TIdent>
        where TResult : IIdentityObject<TIdent>
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return source.Items.Select(node => node.ChangeItem(selector)).ToSelectOperationResult(source.TotalCount);
    }
}
