using System.Linq.Expressions;

namespace Framework.OData.QueryLanguage.Operations;

public static class UnaryOperationExtensions
{
    public static string ToFormatString(this UnaryOperation operation) =>
        operation switch
        {
            UnaryOperation.Not => "!",
            UnaryOperation.Plus => "+",
            UnaryOperation.Negate => "-",
            _ => throw new ArgumentOutOfRangeException(nameof(operation))
        };

    public static ExpressionType ToExpressionType(this UnaryOperation operation) =>
        operation switch
        {
            UnaryOperation.Not => ExpressionType.Not,
            UnaryOperation.Plus => ExpressionType.UnaryPlus,
            UnaryOperation.Negate => ExpressionType.Negate,
            _ => throw new ArgumentOutOfRangeException(nameof(operation))
        };

    public static int GetPriority(this UnaryOperation operation) =>
        operation switch
        {
            UnaryOperation.Not => 10,
            UnaryOperation.Negate => 20,
            UnaryOperation.Plus => 20,
            _ => throw new ArgumentOutOfRangeException(nameof(operation))
        };
}
