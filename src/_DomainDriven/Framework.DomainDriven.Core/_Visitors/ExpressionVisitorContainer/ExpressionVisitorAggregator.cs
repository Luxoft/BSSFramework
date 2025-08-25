using System.Linq.Expressions;

namespace Framework.DomainDriven._Visitors;

public class ExpressionVisitorAggregator : IExpressionVisitorContainer
{
    private readonly IExpressionVisitorContainerItem[] items;

    public ExpressionVisitorAggregator(IEnumerable<IExpressionVisitorContainerItem> items)
    {
        this.items = items.ToArray();
    }

    public ExpressionVisitor Visitor => this.items.SelectMany(item => item.GetVisitors()).ToComposite();
}
