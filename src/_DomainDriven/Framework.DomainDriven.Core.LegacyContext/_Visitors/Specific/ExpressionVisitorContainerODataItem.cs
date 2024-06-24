using System.Linq.Expressions;

namespace Framework.DomainDriven._Visitors;

public class ExpressionVisitorContainerODataItem<TBLLContext, TPersistentDomainObjectBase, TIdent> : IExpressionVisitorContainerItem
        where TBLLContext : class, IHierarchicalObjectExpanderFactoryContainer<TIdent>
{
    private readonly TBLLContext context;

    private readonly IIdPropertyResolver idPropertyResolver;

    public ExpressionVisitorContainerODataItem(TBLLContext context, IIdPropertyResolver idPropertyResolver)
    {
        this.context = context;
        this.idPropertyResolver = idPropertyResolver;
    }

    public IEnumerable<ExpressionVisitor> GetVisitors()
    {
        yield return new OverrideExpandContainsVisitor<TBLLContext, TIdent>(this.context, this.idPropertyResolver.Resolve(typeof(TPersistentDomainObjectBase)));
    }
}
