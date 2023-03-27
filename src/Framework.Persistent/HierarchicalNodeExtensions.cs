using JetBrains.Annotations;

namespace Framework.Persistent;

public static class HierarchicalNodeExtensions
{
    public static List<HierarchicalNode<TResult, TIdent>> ToList<TSource, TResult, TIdent>([NotNull] this IEnumerable<HierarchicalNode<TSource, TIdent>> source, [NotNull] Func<TSource, TResult> selector)
            where TResult : IIdentityObject<TIdent>
            where TSource : IIdentityObject<TIdent>
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return source.Select(v => v.ChangeItem(selector)).ToList();
    }
}
