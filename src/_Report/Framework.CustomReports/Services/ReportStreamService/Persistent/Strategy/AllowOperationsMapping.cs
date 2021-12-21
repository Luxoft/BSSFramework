using System.Collections.Generic;
using System.Linq.Expressions;

using Framework.Configuration.Domain.Reports;

namespace Framework.CustomReports.Services.Persistent
{
    internal static class AllowOperationsMapping
    {
        internal static readonly Dictionary<string, ExpressionType> AllowNullableOperations
            = new Dictionary<string, ExpressionType>
              {
                  { ReportFilter.IsAfterOrNullOperator, ExpressionType.GreaterThan },
                  { ReportFilter.IsNullOrAfterOrEqualOperator, ExpressionType.GreaterThanOrEqual },
                  { ReportFilter.IsBeforeOrNullOperator, ExpressionType.LessThan },
                  { ReportFilter.IsNullOrBeforeOrEqualOperator, ExpressionType.LessThanOrEqual }
              };

        private static readonly Dictionary<string, ExpressionType> AllowOperations =
            new Dictionary<string, ExpressionType>
            {
                { "gt", ExpressionType.GreaterThan },
                { "lt", ExpressionType.LessThan },
                { "le", ExpressionType.LessThanOrEqual },
                { "lte", ExpressionType.LessThanOrEqual },
                { "ge", ExpressionType.GreaterThanOrEqual },
                { "gte", ExpressionType.GreaterThanOrEqual },
                { "eq", ExpressionType.Equal },
                { "ne", ExpressionType.NotEqual },
                { "and", ExpressionType.AndAlso },
                { "or", ExpressionType.OrElse },
                { "neq", ExpressionType.NotEqual },
            };

        internal static ExpressionType? TryGetValue(string binaryOperator)
        {
            if (AllowOperations.TryGetValue(binaryOperator, out var expressionType))
            {
                return expressionType;
            }

            return null;
        }
    }
}
