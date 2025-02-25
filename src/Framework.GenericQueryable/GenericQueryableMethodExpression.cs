using System.Linq.Expressions;

namespace Framework.GenericQueryable;

public class GenericQueryableMethodExpression(LambdaExpression callExpression) : Expression
{
    public LambdaExpression CallExpression { get; } = callExpression;
}
