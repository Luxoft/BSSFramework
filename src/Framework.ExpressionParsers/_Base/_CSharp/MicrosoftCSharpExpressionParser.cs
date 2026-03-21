using System.Linq.Expressions;

namespace Framework.ExpressionParsers;

internal class MicrosoftCSharpExpressionParser : INativeBodyExpressionParser
{
    public Expression Parse(ParameterExpression[] parameters, Type resultType, string expression)
    {
        var resultExpression = new MicrosoftCSharpExpressionParserImplement.ExpressionParserService(parameters, expression, []).Parse(resultType);

        return Expression.Lambda(resultExpression, parameters);
    }
}
