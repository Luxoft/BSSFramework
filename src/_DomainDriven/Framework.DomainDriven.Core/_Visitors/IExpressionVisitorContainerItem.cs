using System.Linq.Expressions;

namespace Framework.DomainDriven;

public interface IExpressionVisitorContainerItem
{
    IEnumerable<ExpressionVisitor> GetVisitors();
}
