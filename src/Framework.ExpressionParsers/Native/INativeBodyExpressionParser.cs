using System.Linq.Expressions;

namespace Framework.ExpressionParsers.Native;

public interface INativeBodyExpressionParser
{
    Expression Parse(ParameterExpression[] parameters, Type resultType, string expression);
}
