using System.Linq.Expressions;

using Framework.Core;

namespace Framework.Database.ExpressionVisitorContainer;

public class ExpressionVisitorAggregator(IEnumerable<IExpressionVisitorContainerItem> items) : IExpressionVisitorContainer
{
    private readonly IExpressionVisitorContainerItem[] items = items.ToArray();

    public ExpressionVisitor Visitor => this.items.SelectMany(item => item.GetVisitors()).ToComposite();
}
