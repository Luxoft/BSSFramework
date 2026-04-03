using System.Linq.Expressions;

namespace Framework.Database;

public class ExpressionVisitorContainer(ExpressionVisitor visitor) : IExpressionVisitorContainer
{
    public ExpressionVisitor Visitor { get; } = visitor;
}
