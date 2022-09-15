using System.Collections.Generic;
using System.Linq.Expressions;

using Framework.DomainDriven.BLL;

namespace Framework.DomainDriven;

public class ExpressionVisitorContainerDomainIdentItem<TPersistentDomainObjectBase, TIdent> : IExpressionVisitorContainerItem
{
    private readonly IIdPropertyResolver idPropertyResolver;

    public ExpressionVisitorContainerDomainIdentItem(IIdPropertyResolver idPropertyResolver)
    {
        this.idPropertyResolver = idPropertyResolver;
    }

    public IEnumerable<ExpressionVisitor> GetVisitors()
    {
        yield return OverrideInstanceContainsIdentMethodVisitor<TIdent>.HashSet;
        yield return OverrideInstanceContainsIdentMethodVisitor<TIdent>.CollectionInterface;
        // TODO gtsaplin: remove OverrideHashSetVisitor, NH4.0 and above support HashSet
        yield return OverrideHashSetVisitor<TIdent>.Value;

        var idProperty = this.idPropertyResolver.Resolve(typeof(TPersistentDomainObjectBase));
        yield return OverrideListContainsVisitor<TIdent>.GetOrCreate(idProperty);
        yield return OverrideEqualsDomainObjectVisitor<TIdent>.GetOrCreate(idProperty);
        yield return OverrideIdEqualsMethodVisitor<TIdent>.GetOrCreate(idProperty);
    }
}
