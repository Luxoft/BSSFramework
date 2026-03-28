using System.Linq.Expressions;

namespace Framework.Database.ExpressionVisitorContainer;

public interface IExpressionVisitorContainerItem
{
    IEnumerable<ExpressionVisitor> GetVisitors();
}
