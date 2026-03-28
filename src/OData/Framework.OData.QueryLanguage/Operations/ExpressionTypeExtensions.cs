using SExpressions = System.Linq.Expressions;

namespace Framework.OData.QueryLanguage.Operations;

public static class ExpressionTypeExtensions
{
    public static BinaryOperation ToBinaryOperation(this SExpressions.ExpressionType expressionType) =>
        expressionType switch
        {
            SExpressions.ExpressionType.Equal => BinaryOperation.Equal,
            SExpressions.ExpressionType.GreaterThanOrEqual => BinaryOperation.GreaterThanOrEqual,
            SExpressions.ExpressionType.LessThanOrEqual => BinaryOperation.LessThanOrEqual,
            SExpressions.ExpressionType.NotEqual => BinaryOperation.NotEqual,
            SExpressions.ExpressionType.GreaterThan => BinaryOperation.GreaterThan,
            SExpressions.ExpressionType.LessThan => BinaryOperation.LessThan,
            SExpressions.ExpressionType.Add => BinaryOperation.Add,
            SExpressions.ExpressionType.Subtract => BinaryOperation.Subtract,
            SExpressions.ExpressionType.OrElse => BinaryOperation.OrElse,
            SExpressions.ExpressionType.AndAlso => BinaryOperation.AndAlso,
            _ => throw new ArgumentOutOfRangeException(nameof(expressionType))
        };
}
