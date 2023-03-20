using System;

using SExpressions = System.Linq.Expressions;

namespace Framework.QueryLanguage;

public static class ExpressionTypeExtensions
{
    public static BinaryOperation ToBinaryOperation(this SExpressions.ExpressionType expressionType)
    {
        switch (expressionType)
        {
            case SExpressions.ExpressionType.Equal:
                return BinaryOperation.Equal;

            case SExpressions.ExpressionType.GreaterThanOrEqual:
                return BinaryOperation.GreaterThanOrEqual;

            case SExpressions.ExpressionType.LessThanOrEqual:
                return BinaryOperation.LessThanOrEqual;

            case SExpressions.ExpressionType.NotEqual:
                return BinaryOperation.NotEqual;

            case SExpressions.ExpressionType.GreaterThan:
                return BinaryOperation.GreaterThan;

            case SExpressions.ExpressionType.LessThan:
                return BinaryOperation.LessThan;

            case SExpressions.ExpressionType.Add:
                return BinaryOperation.Add;

            case SExpressions.ExpressionType.Subtract:
                return BinaryOperation.Subtract;

            case SExpressions.ExpressionType.OrElse:
                return BinaryOperation.OrElse;

            case SExpressions.ExpressionType.AndAlso:
                return BinaryOperation.AndAlso;

            default:
                throw new ArgumentOutOfRangeException(nameof(expressionType));
        }
    }
}
