using Framework.Core;
using Framework.Persistent;

using JetBrains.Annotations;

namespace Framework.HierarchicalExpand;

public static class HierarchicalExtensions
{
    public static T GetRoot<T>(this T source)
            where T : class, IParentSource<T>
    {
        return source.GetAllElements(v => v.Parent).Last();
    }

    public static IEnumerable<T> GetAllParents<T>(this T source, bool skipFirstElement = false)
            where T : class, IParentSource<T>
    {
        return source.GetAllElements(v => v.Parent, skipFirstElement);
    }

    public static IEnumerable<T> GetAllChildren<T>(this T source, bool skipFirstElement = false)
            where T : class, IChildrenSource<T>
    {
        return source.GetAllElements(v => v.Children, skipFirstElement);
    }

    public static IEnumerable<T> GetAllParentsM<T>(this IEnumerable<T> source, bool skipFirstElement = false)
            where T : class, IParentSource<T>
    {
        return source.SelectMany(el => el.GetAllParents(skipFirstElement));
    }

    public static IEnumerable<T> GetAllChildrenM<T>(this IEnumerable<T> source, bool skipFirstElement = false)
            where T : class, IChildrenSource<T>
    {
        return source.SelectMany(el => el.GetAllChildren(skipFirstElement));
    }

    public static IEnumerable<TSource> Expand<TSource>([NotNull] this TSource source, HierarchicalExpandType expandType)
            where TSource : class, IHierarchicalSource<TSource>
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var parents = expandType.HasFlag(HierarchicalExpandType.Parents) ? source.GetAllParents(true) : new TSource[0];

        var children = expandType.HasFlag(HierarchicalExpandType.Children) ? source.GetAllChildren(true) : new TSource[0];

        return parents.Concat(new[] { source }).Concat(children);
    }

    /// <summary>
    /// Check source in expandable values
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="source"></param>
    /// <param name="id"></param>
    /// <param name="expandType">Direction of expand path</param>
    /// <returns></returns>
    public static bool IsExpandableBy<TSource, TIdent>([NotNull] this TSource source, TIdent id, HierarchicalExpandType expandType)
            where TSource : class, IHierarchicalSource<TSource>, IIdentityObject<TIdent>
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.Expand(expandType.Reverse()).Select(s => s.Id).Contains(id);
    }
}
