using System;
using System.Linq.Expressions;

namespace Framework.ExpressionParsers
{
    public interface INativeBodyExpressionParser
    {
        Expression Parse(ParameterExpression[] parameters, Type resultType, string expression);
    }
}