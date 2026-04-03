using System.Linq.Expressions;

namespace Framework.Database;

public interface IExpressionVisitorContainer
{
    public const string ElementKey = "Element";

    ExpressionVisitor Visitor { get; }
}
