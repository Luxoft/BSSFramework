using System.Linq.Expressions;

using Framework.Core;
using Framework.Persistent;
using Framework.QueryableSource;

namespace Framework.HierarchicalExpand;

public class HierarchicalObjectAncestorLinkExpander<TDomainObject, TDomainObjectAncestorLink, TDomainObjectAncestorChildLink, TIdent> : IHierarchicalObjectExpander<TIdent>, IHierarchicalObjectQueryableExpander<TIdent>
        where TDomainObject : class, IHierarchicalPersistentDomainObjectBase<TDomainObject, TIdent>
        where TDomainObjectAncestorLink : class, IHierarchicalAncestorLink<TDomainObject, TDomainObjectAncestorChildLink, TIdent>
        where TDomainObjectAncestorChildLink : class, IHierarchicalToAncestorOrChildLink<TDomainObject, TIdent>
        where TIdent : struct
{
    private readonly IQueryableSource queryableSource;

    public HierarchicalObjectAncestorLinkExpander(IQueryableSource queryableSource)
    {
        this.queryableSource = queryableSource ?? throw new ArgumentNullException(nameof(queryableSource));
    }

    public IEnumerable<TIdent> Expand(IEnumerable<TIdent> idents, HierarchicalExpandType expandType)
    {
        if (idents == null) throw new ArgumentNullException(nameof(idents));

        if (idents is IQueryable<TIdent>)
        {
            return this.ExpandQueryable(idents as IQueryable<TIdent>, expandType);
        }
        else
        {
            return this.ExpandEnumerable(idents, expandType);
        }
    }

    public Dictionary<TIdent, TIdent> ExpandWithParents(IEnumerable<TIdent> idents, HierarchicalExpandType expandType)
    {
        return this.ExpandWithParentsImplementation(idents.ToHashSet(), expandType);
    }

    public Dictionary<TIdent, TIdent> ExpandWithParents(IQueryable<TIdent> idents, HierarchicalExpandType expandType)
    {
        return this.ExpandWithParentsImplementation(idents, expandType);
    }

    private Dictionary<TIdent, TIdent> ExpandWithParentsImplementation(IEnumerable<TIdent> idents, HierarchicalExpandType expandType)
    {
        return this.ExpandDomainObject(idents, expandType)
                   .Select(domainObject => new { Id = domainObject.Id, ParentId = (TIdent?)domainObject.Parent.Id })
                   .Distinct()
                   .ToDictionary(pair => pair.Id, pair => pair.ParentId.GetValueOrDefault());
    }

    private IQueryable<TDomainObject> ExpandDomainObject(
            IEnumerable<TIdent> idents,
            HierarchicalExpandType expandType)
    {
        switch (expandType)
        {
            case HierarchicalExpandType.None:

                return from domainObject in this.queryableSource.GetQueryable<TDomainObject>()
                       where idents.Contains(domainObject.Id)
                       select domainObject;

            case HierarchicalExpandType.Children:

                return from link in this.queryableSource.GetQueryable<TDomainObjectAncestorLink>()
                       where idents.Contains(link.Ancestor.Id)
                       select link.Child;

            case HierarchicalExpandType.Parents:

                return from link in this.queryableSource.GetQueryable<TDomainObjectAncestorLink>()
                       where idents.Contains(link.Child.Id)
                       select link.Ancestor;

            case HierarchicalExpandType.All:

                return from link in this.queryableSource.GetQueryable<TDomainObjectAncestorChildLink>()
                       where idents.Contains(link.Source.Id)
                       select link.ChildOrAncestor;

            default:
                throw new ArgumentOutOfRangeException(nameof(expandType));
        }
    }

    public IEnumerable<TIdent> ExpandEnumerable(IEnumerable<TIdent> baseIdents, HierarchicalExpandType expandType)
    {
        if (baseIdents == null) throw new ArgumentNullException(nameof(baseIdents));

        var ancestorLinkQueryable = this.queryableSource.GetQueryable<TDomainObjectAncestorLink>();

        var idents = baseIdents.ToHashSet();

        switch (expandType)
        {
            case HierarchicalExpandType.None:

                return idents;

            case HierarchicalExpandType.Children:

                return ancestorLinkQueryable.Where(link => idents.Contains(link.Ancestor.Id)).Select(link => link.Child.Id);

            case HierarchicalExpandType.Parents:

                return ancestorLinkQueryable.Where(link => idents.Contains(link.Child.Id)).Select(link => link.Ancestor.Id);

            case HierarchicalExpandType.All:

                return this.queryableSource.GetQueryable<TDomainObjectAncestorChildLink>()
                           .Where(z => idents.Contains(z.Source.Id))
                           .Select(z => z.ChildOrAncestor.Id);

            default:
                throw new ArgumentOutOfRangeException(nameof(expandType));
        }
    }

    public IQueryable<TIdent> ExpandQueryable(IQueryable<TIdent> idents, HierarchicalExpandType expandType)
    {
        if (idents == null) throw new ArgumentNullException(nameof(idents));

        var ancestorLinkQueryable = this.queryableSource.GetQueryable<TDomainObjectAncestorLink>();

        switch (expandType)
        {
            case HierarchicalExpandType.None:

                return idents;

            case HierarchicalExpandType.Children:

                return ancestorLinkQueryable.Where(link => idents.Contains(link.Ancestor.Id)).Select(link => link.Child.Id);

            case HierarchicalExpandType.Parents:

                return ancestorLinkQueryable.Where(link => idents.Contains(link.Child.Id)).Select(link => link.Ancestor.Id);

            case HierarchicalExpandType.All:

                return this.queryableSource.GetQueryable<TDomainObjectAncestorChildLink>()
                           .Where(z => idents.Contains(z.Source.Id))
                           .Select(z => z.ChildOrAncestor.Id);

            default:
                throw new ArgumentOutOfRangeException(nameof(expandType));
        }
    }

    public Expression<Func<IEnumerable<TIdent>, IEnumerable<TIdent>>> GetExpandExpression(HierarchicalExpandType expandType)
    {
        var ancestorLinkQueryable = this.queryableSource.GetQueryable<TDomainObjectAncestorLink>();

        switch (expandType)
        {
            case HierarchicalExpandType.None:

                return idents => idents;

            case HierarchicalExpandType.Children:

                return idents => ancestorLinkQueryable.Where(link => idents.Contains(link.Ancestor.Id)).Select(link => link.Child.Id);

            case HierarchicalExpandType.Parents:

                return idents => ancestorLinkQueryable.Where(link => idents.Contains(link.Child.Id)).Select(link => link.Ancestor.Id);

            case HierarchicalExpandType.All:

                return idents => this.queryableSource.GetQueryable<TDomainObjectAncestorChildLink>()
                                     .Where(z => idents.Contains(z.Source.Id))
                                     .Select(z => z.ChildOrAncestor.Id);

            default:
                throw new ArgumentOutOfRangeException(nameof(expandType));
        }
    }

    public Expression<Func<TIdent, IEnumerable<TIdent>>> TryGetSingleExpandExpression(HierarchicalExpandType expandType)
    {
        return this.TryGetSingleExpandExpressionInternal(expandType).Maybe(expr => expr.ExpandConst().InlineEval());
    }

    private Expression<Func<TIdent, IEnumerable<TIdent>>> TryGetSingleExpandExpressionInternal(HierarchicalExpandType expandType)
    {
        var ancestorLinkQueryable = this.queryableSource.GetQueryable<TDomainObjectAncestorLink>();

        var eqIdentsExpr = ExpressionHelper.GetEquality<TIdent>();

        switch (expandType)
        {
            case HierarchicalExpandType.None:

                return null;

            case HierarchicalExpandType.Children:

                return ident => ancestorLinkQueryable.Where(link => eqIdentsExpr.Eval(ident, link.Ancestor.Id)).Select(link => link.Child.Id);

            case HierarchicalExpandType.Parents:

                return ident => ancestorLinkQueryable.Where(link => eqIdentsExpr.Eval(ident, link.Child.Id)).Select(link => link.Ancestor.Id);

            case HierarchicalExpandType.All:

                return ident => this.queryableSource.GetQueryable<TDomainObjectAncestorChildLink>()

                                    .Where(z => eqIdentsExpr.Eval(ident, z.Source.Id))
                                    .Select(z => z.ChildOrAncestor.Id);

            default:
                throw new ArgumentOutOfRangeException(nameof(expandType));
        }
    }
}
