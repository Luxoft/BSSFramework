using System.Linq.Expressions;

namespace Framework.GenericQueryable;

public class GenericQueryableExecuteExpression(LambdaExpression callExpression) : Expression
{
    public LambdaExpression CallExpression { get; } = callExpression;
}
