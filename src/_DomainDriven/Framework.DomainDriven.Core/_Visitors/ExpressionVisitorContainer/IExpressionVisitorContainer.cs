using System.Linq.Expressions;

namespace Framework.DomainDriven;

public interface IExpressionVisitorContainer
{
    ExpressionVisitor Visitor { get; }
}

