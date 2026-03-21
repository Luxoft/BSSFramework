using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Framework.QueryLanguage;

[DataContract]
public static class BinaryOperationExtensions
{
    public static string ToFormatString(this BinaryOperation operation) =>
        operation switch
        {
            BinaryOperation.Equal => "==",
            BinaryOperation.GreaterThanOrEqual => ">=",
            BinaryOperation.LessThanOrEqual => "<=",
            BinaryOperation.NotEqual => "!=",
            BinaryOperation.GreaterThan => ">",
            BinaryOperation.LessThan => "<",
            BinaryOperation.Add => "+",
            BinaryOperation.Subtract => "-",
            BinaryOperation.Mul => "*",
            BinaryOperation.Div => "/",
            BinaryOperation.Mod => "%",
            BinaryOperation.OrElse => "||",
            BinaryOperation.AndAlso => "&&",
            _ => throw new ArgumentOutOfRangeException(nameof(operation))
        };

    public static ExpressionType ToExpressionType(this BinaryOperation operation) =>
        operation switch
        {
            BinaryOperation.Equal => ExpressionType.Equal,
            BinaryOperation.GreaterThanOrEqual => ExpressionType.GreaterThanOrEqual,
            BinaryOperation.LessThanOrEqual => ExpressionType.LessThanOrEqual,
            BinaryOperation.NotEqual => ExpressionType.NotEqual,
            BinaryOperation.GreaterThan => ExpressionType.GreaterThan,
            BinaryOperation.LessThan => ExpressionType.LessThan,
            BinaryOperation.Add => ExpressionType.Add,
            BinaryOperation.Subtract => ExpressionType.Subtract,
            BinaryOperation.Mul => ExpressionType.Multiply,
            BinaryOperation.Div => ExpressionType.Divide,
            BinaryOperation.Mod => ExpressionType.Modulo,
            BinaryOperation.OrElse => ExpressionType.OrElse,
            BinaryOperation.AndAlso => ExpressionType.AndAlso,
            _ => throw new ArgumentOutOfRangeException(nameof(operation))
        };

    public static int GetPriority(this BinaryOperation operation) =>
        operation switch
        {
            BinaryOperation.Equal => 0,
            BinaryOperation.GreaterThanOrEqual => 0,
            BinaryOperation.LessThanOrEqual => 0,
            BinaryOperation.NotEqual => 0,
            BinaryOperation.GreaterThan => 0,
            BinaryOperation.LessThan => 0,
            BinaryOperation.Add => 1,
            BinaryOperation.Subtract => 1,
            BinaryOperation.Mul => 2,
            BinaryOperation.Div => 2,
            BinaryOperation.Mod => 2,
            BinaryOperation.OrElse => -1,
            BinaryOperation.AndAlso => -1,
            _ => throw new ArgumentOutOfRangeException(nameof(operation))
        };
}
