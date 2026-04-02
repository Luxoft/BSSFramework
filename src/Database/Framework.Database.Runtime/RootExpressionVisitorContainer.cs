using System.Linq.Expressions;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database;

public class RootExpressionVisitorContainer([FromKeyedServices(IExpressionVisitorContainer.ElementKey)] IEnumerable<IExpressionVisitorContainer> items)
    : ExpressionVisitorAggregator
{
    protected override IEnumerable<ExpressionVisitor> GetVisitors() => items.Select(item => item.Visitor);
}
