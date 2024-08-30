﻿using Framework.Persistent;
using Framework.QueryableSource;

namespace Framework.HierarchicalExpand;

public class HierarchicalObjectLayerExpander<TDomainObject, TIdent>(IQueryableSource queryableSource) : IHierarchicalObjectExpander<TIdent>
    where TDomainObject : class, IHierarchicalPersistentDomainObjectBase<TDomainObject, TIdent>
    where TIdent : struct
{
    private readonly IQueryableSource queryableSource = queryableSource ?? throw new ArgumentNullException(nameof(queryableSource));

    public IEnumerable<TIdent> Expand(IEnumerable<TIdent> idents, HierarchicalExpandType expandType)
    {
        if (idents == null) throw new ArgumentNullException(nameof(idents));

        return this.queryableSource.GetQueryable<TDomainObject>().Expand(idents, expandType);
    }

    public Dictionary<TIdent, TIdent> ExpandWithParents(IEnumerable<TIdent> idents, HierarchicalExpandType expandType)
    {
        return this.ExpandWithParentsImplementation(idents, expandType);
    }

    public Dictionary<TIdent, TIdent> ExpandWithParents(IQueryable<TIdent> idents, HierarchicalExpandType expandType)
    {
        return this.ExpandWithParentsImplementation(idents, expandType);
    }

    private Dictionary<TIdent, TIdent> ExpandWithParentsImplementation(IEnumerable<TIdent> idents, HierarchicalExpandType expandType)
    {
        var expandedIdents = this.Expand(idents, expandType).ToList();

        var request = from domainObject in this.queryableSource.GetQueryable<TDomainObject>()

                      where expandedIdents.Contains(domainObject.Id)

                      select new
                             {
                                     Id = domainObject.Id,

                                     ParentId = (TIdent?)domainObject.Parent.Id
                             };

        return request.ToDictionary(pair => pair.Id, pair => pair.ParentId.GetValueOrDefault());
    }
}
