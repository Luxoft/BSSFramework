using System.Linq.Expressions;

namespace Framework.Application._Visitors.ExpressionVisitorContainer;

public interface IExpressionVisitorContainerItem
{
    IEnumerable<ExpressionVisitor> GetVisitors();
}
