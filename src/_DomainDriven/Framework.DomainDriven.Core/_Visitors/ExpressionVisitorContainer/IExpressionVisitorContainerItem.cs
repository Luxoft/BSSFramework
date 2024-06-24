using System.Linq.Expressions;

namespace Framework.DomainDriven._Visitors;

public interface IExpressionVisitorContainerItem
{
    IEnumerable<ExpressionVisitor> GetVisitors();
}
