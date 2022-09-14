using System.Collections.Generic;
using System.Linq.Expressions;

using Framework.DomainDriven.BLL;

namespace Framework.DomainDriven;

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
