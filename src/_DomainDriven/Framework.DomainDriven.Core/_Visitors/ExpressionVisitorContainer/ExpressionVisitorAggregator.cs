using System.Linq.Expressions;

using Framework.Core;

namespace Framework.DomainDriven;

public class ExpressionVisitorAggregator : IExpressionVisitorContainer
{
    private readonly IExpressionVisitorContainerItem[] items;

    public ExpressionVisitorAggregator(IEnumerable<IExpressionVisitorContainerItem> items)
    {
        this.items = items.ToArray();
    }

    public ExpressionVisitor Visitor => this.items.SelectMany(item => item.GetVisitors()).ToComposite();
}
