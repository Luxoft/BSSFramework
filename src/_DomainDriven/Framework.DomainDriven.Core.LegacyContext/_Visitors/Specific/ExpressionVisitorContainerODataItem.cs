using System.Linq.Expressions;

using Framework.DomainDriven.BLL;
using Framework.HierarchicalExpand;
using Framework.Persistent;

namespace Framework.DomainDriven;

public class ExpressionVisitorContainerODataItem<TPersistentDomainObjectBase, TIdent> : IExpressionVisitorContainerItem
    where TPersistentDomainObjectBase : IIdentityObject<TIdent>
{
    private readonly IHierarchicalObjectExpanderFactory<TIdent> hierarchicalObjectExpanderFactory;

    private readonly IIdPropertyResolver idPropertyResolver;

    public ExpressionVisitorContainerODataItem(IHierarchicalObjectExpanderFactory<TIdent> hierarchicalObjectExpanderFactory, IIdPropertyResolver idPropertyResolver)
    {
        this.hierarchicalObjectExpanderFactory = hierarchicalObjectExpanderFactory;
        this.idPropertyResolver = idPropertyResolver;
    }

    public IEnumerable<ExpressionVisitor> GetVisitors()
    {
        yield return new OverrideExpandContainsVisitor<TIdent>(this.hierarchicalObjectExpanderFactory, this.idPropertyResolver.Resolve(typeof(TPersistentDomainObjectBase)));
    }
}
