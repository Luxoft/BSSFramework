using System.Linq.Expressions;

namespace Framework.DomainDriven._Visitors;

public class ExpressionVisitorContainerDomainIdentItem<TPersistentDomainObjectBase, TIdent>(IIdPropertyResolver idPropertyResolver)
    : IExpressionVisitorContainerItem
{
    public IEnumerable<ExpressionVisitor> GetVisitors()
    {
        yield return OverrideInstanceContainsIdentMethodVisitor<TIdent>.HashSet;
        yield return OverrideInstanceContainsIdentMethodVisitor<TIdent>.CollectionInterface;
        // TODO gtsaplin: remove OverrideHashSetVisitor, NH4.0 and above support HashSet
        yield return OverrideHashSetVisitor<TIdent>.Value;

        var idProperty = idPropertyResolver.Resolve(typeof(TPersistentDomainObjectBase));
        yield return OverrideListContainsVisitor<TIdent>.GetOrCreate(idProperty);
        yield return OverrideEqualsDomainObjectVisitor<TIdent>.GetOrCreate(idProperty);
        yield return OverrideIdEqualsMethodVisitor<TIdent>.GetOrCreate(idProperty);
    }
}
