using System.Linq.Expressions;

namespace Framework.Application._Visitors.ExpressionVisitorContainer;

public interface IExpressionVisitorContainer
{
    ExpressionVisitor Visitor { get; }
}

