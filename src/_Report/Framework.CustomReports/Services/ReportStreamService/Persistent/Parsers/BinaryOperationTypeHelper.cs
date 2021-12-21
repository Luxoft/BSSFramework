using System.Collections.Generic;

using Framework.Configuration.Domain.Reports;
using Framework.QueryLanguage;

namespace Framework.CustomReports.Services
{
    internal static class BinaryOperationTypeHelper
    {
        internal static readonly Dictionary<string, BinaryOperation> AllowOperations
            = new Dictionary<string, BinaryOperation>
              {
                  { "gt", BinaryOperation.GreaterThan },
                  { "lt", BinaryOperation.LessThan },
                  { "le", BinaryOperation.LessThanOrEqual },
                  { "lte", BinaryOperation.LessThanOrEqual },
                  { "ge", BinaryOperation.GreaterThanOrEqual },
                  { "gte", BinaryOperation.GreaterThanOrEqual },
                  { "eq", BinaryOperation.Equal },
                  { "eqn", BinaryOperation.Equal },
                  { "ne", BinaryOperation.NotEqual },
                  { "neq", BinaryOperation.NotEqual },
                  { "and", BinaryOperation.AndAlso },
                  { "or", BinaryOperation.OrElse },
              };

        internal static readonly Dictionary<string, BinaryOperation> AllowNullableOperations
            = new Dictionary<string, BinaryOperation>
              {
                  { ReportFilter.IsAfterOrNullOperator, BinaryOperation.GreaterThan },
                  { ReportFilter.IsNullOrAfterOrEqualOperator, BinaryOperation.GreaterThanOrEqual },
                  { ReportFilter.IsBeforeOrNullOperator, BinaryOperation.LessThan },
                  { ReportFilter.IsNullOrBeforeOrEqualOperator, BinaryOperation.LessThanOrEqual }
              };
    }
}
