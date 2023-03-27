using Framework.Core;
using Framework.Persistent;

namespace Framework.HierarchicalExpand;

public static class QueryableExpandExtensions
{
    public static IEnumerable<TIdent> Expand<TDomainObject, TIdent>(this IQueryable<TDomainObject> queryable, IEnumerable<TIdent> idents, HierarchicalExpandType expandType)
            where TDomainObject : class, IHierarchicalPersistentDomainObjectBase<TDomainObject, TIdent>
    {
        if (queryable == null) throw new ArgumentNullException(nameof(queryable));
        if (idents == null) throw new ArgumentNullException(nameof(idents));

        if (expandType == HierarchicalExpandType.None)
        {
            return idents.ToHashSet();
        }
        else
        {
            return queryable.ExpandMany(idents, expandType).SelectMany(v => v).ToHashSet();
        }
    }

    private static IEnumerable<HashSet<TIdent>> ExpandMany<TDomainObject, TIdent>(this IQueryable<TDomainObject> queryable, IEnumerable<TIdent> idents, HierarchicalExpandType expandType)
            where TDomainObject : class, IHierarchicalPersistentDomainObjectBase<TDomainObject, TIdent>
    {
        if (queryable == null) throw new ArgumentNullException(nameof(queryable));
        if (idents == null) throw new ArgumentNullException(nameof(idents));

        var cachedIdents = idents.ToHashSet();

        yield return cachedIdents;

        yield return expandType.HasFlag(HierarchicalExpandType.Parents) ? queryable.ExpandMasters(cachedIdents) : new HashSet<TIdent>();

        yield return expandType.HasFlag(HierarchicalExpandType.Children) ? queryable.ExpandChildren(cachedIdents) : new HashSet<TIdent>();
    }

    private static HashSet<TIdent> ExpandMasters<TDomainObject, TIdent>(this IQueryable<TDomainObject> queryable, IEnumerable<TIdent> baseIdents)
            where TDomainObject : class, IHierarchicalPersistentDomainObjectBase<TDomainObject, TIdent>
    {
        if (queryable == null) throw new ArgumentNullException(nameof(queryable));
        if (baseIdents == null) throw new ArgumentNullException(nameof(baseIdents));

        var currentIdents = baseIdents.ToHashSet();

        for (var roots = new HashSet<TIdent>(currentIdents); roots.Any(); currentIdents.AddRange(roots))
        {
            roots = queryable.Where(v =>
                                            !currentIdents.Contains(v.Id) && v.Children.Any(children => roots.Contains(children.Id))).Select(v => v.Id).ToHashSet();
        }

        return currentIdents;
    }

    private static HashSet<TIdent> ExpandChildren<TDomainObject, TIdent>(this IQueryable<TDomainObject> queryable, IEnumerable<TIdent> baseIdents)
            where TDomainObject : class, IHierarchicalPersistentDomainObjectBase<TDomainObject, TIdent>
    {
        var currentIdents = baseIdents.ToHashSet();

        for (var children = new HashSet<TIdent>(currentIdents); children.Any(); currentIdents.AddRange(children))
        {
            children = queryable.Where(v =>
                                               !currentIdents.Contains(v.Id) && (v.Parent != null && children.Contains(v.Parent.Id))).Select(v => v.Id).ToHashSet();
        }

        return currentIdents;
    }
}
