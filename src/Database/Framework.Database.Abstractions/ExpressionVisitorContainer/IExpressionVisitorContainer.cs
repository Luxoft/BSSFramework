using System.Linq.Expressions;

namespace Framework.Database.ExpressionVisitorContainer;

public interface IExpressionVisitorContainer
{
    ExpressionVisitor Visitor { get; }
}

