using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Framework.QueryLanguage;

[DataContract]
public static class BinaryOperationExtensions
{
    public static string ToFormatString(this BinaryOperation operation)
    {
        switch (operation)
        {
            case BinaryOperation.Equal:
                return "==";

            case BinaryOperation.GreaterThanOrEqual:
                return ">=";

            case BinaryOperation.LessThanOrEqual:
                return "<=";

            case BinaryOperation.NotEqual:
                return "!=";

            case BinaryOperation.GreaterThan:
                return ">";

            case BinaryOperation.LessThan:
                return "<";

            case BinaryOperation.Add:
                return "+";

            case BinaryOperation.Subtract:
                return "-";

            case BinaryOperation.Mul:
                return "*";

            case BinaryOperation.Div:
                return "/";

            case BinaryOperation.Mod:
                return "%";

            case BinaryOperation.OrElse:
                return "||";

            case BinaryOperation.AndAlso:
                return "&&";

            default:
                throw new ArgumentOutOfRangeException(nameof(operation));
        }
    }

    public static ExpressionType ToExpressionType(this BinaryOperation operation)
    {
        switch (operation)
        {
            case BinaryOperation.Equal:
                return ExpressionType.Equal;

            case BinaryOperation.GreaterThanOrEqual:
                return ExpressionType.GreaterThanOrEqual;

            case BinaryOperation.LessThanOrEqual:
                return ExpressionType.LessThanOrEqual;

            case BinaryOperation.NotEqual:
                return ExpressionType.NotEqual;

            case BinaryOperation.GreaterThan:
                return ExpressionType.GreaterThan;

            case BinaryOperation.LessThan:
                return ExpressionType.LessThan;

            case BinaryOperation.Add:
                return ExpressionType.Add;

            case BinaryOperation.Subtract:
                return ExpressionType.Subtract;

            case BinaryOperation.Mul:
                return ExpressionType.Multiply;

            case BinaryOperation.Div:
                return ExpressionType.Divide;

            case BinaryOperation.Mod:
                return ExpressionType.Modulo;

            case BinaryOperation.OrElse:
                return ExpressionType.OrElse;

            case BinaryOperation.AndAlso:
                return ExpressionType.AndAlso;

            default:
                throw new ArgumentOutOfRangeException(nameof(operation));
        }
    }

    public static int GetPriority(this BinaryOperation operation)
    {
        switch (operation)
        {
            case BinaryOperation.Equal:
                return 0;

            case BinaryOperation.GreaterThanOrEqual:
                return 0;

            case BinaryOperation.LessThanOrEqual:
                return 0;

            case BinaryOperation.NotEqual:
                return 0;

            case BinaryOperation.GreaterThan:
                return 0;

            case BinaryOperation.LessThan:
                return 0;

            case BinaryOperation.Add:
                return 1;

            case BinaryOperation.Subtract:
                return 1;

            case BinaryOperation.Mul:
                return 2;

            case BinaryOperation.Div:
                return 2;

            case BinaryOperation.Mod:
                return 2;

            case BinaryOperation.OrElse:
                return -1;

            case BinaryOperation.AndAlso:
                return -1;

            //case BinaryOperation.Pow: Правоассоциативная операция, приоритет выше, чем у унарных
            //    return 40;

            default:
                throw new ArgumentOutOfRangeException(nameof(operation));
        }
    }
}
