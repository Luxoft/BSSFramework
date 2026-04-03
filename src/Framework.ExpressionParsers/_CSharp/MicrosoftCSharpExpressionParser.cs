using System.Linq.Expressions;
using Framework.ExpressionParsers.Native;

namespace Framework.ExpressionParsers._CSharp;

internal class MicrosoftCSharpExpressionParser : INativeBodyExpressionParser
{
    public Expression Parse(ParameterExpression[] parameters, Type resultType, string expression)
    {
        var resultExpression = new MicrosoftCSharpExpressionParserImplement.ExpressionParserService(parameters, expression, []).Parse(resultType);

        return Expression.Lambda(resultExpression, parameters);
    }
}
