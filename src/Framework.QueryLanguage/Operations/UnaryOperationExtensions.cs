using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Framework.QueryLanguage
{
    [DataContract]
    public static class UnaryOperationExtensions
    {
        public static string ToFormatString(this UnaryOperation operation)
        {
            switch (operation)
            {
                case UnaryOperation.Not:
                    return "!";

                case UnaryOperation.Plus:
                    return "+";

                case UnaryOperation.Negate:
                    return "-";

                default:
                    throw new ArgumentOutOfRangeException(nameof(operation));
            }
        }

        public static ExpressionType ToExpressionType(this UnaryOperation operation)
        {
            switch (operation)
            {
                case UnaryOperation.Not:
                    return ExpressionType.Not;

                case UnaryOperation.Plus:
                    return ExpressionType.UnaryPlus;

                case UnaryOperation.Negate:
                    return ExpressionType.Negate;


                default:
                    throw new ArgumentOutOfRangeException(nameof(operation));
            }
        }

        public static int GetPriority(this UnaryOperation operation)
        {
            switch (operation)
            {
                case UnaryOperation.Not:
                    return 10;

                case UnaryOperation.Negate:
                    return 20;

                case UnaryOperation.Plus:
                    return 20;

                default:
                    throw new ArgumentOutOfRangeException(nameof(operation));
            }
        }
    }
}