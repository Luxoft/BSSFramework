using System.Linq;
using System.Linq.Expressions;

using Framework.Core;

namespace Framework.DomainDriven;

public class ExpressionVisitorAggregator : IExpressionVisitorContainer
{
    private readonly IExpressionVisitorContainerItem[] items;

    protected ExpressionVisitorAggregator(IExpressionVisitorContainerItem[] items)
    {
        this.items = items;
    }

    public ExpressionVisitor Visitor => this.items.SelectMany(item => item.GetVisitors()).ToComposite();
}
