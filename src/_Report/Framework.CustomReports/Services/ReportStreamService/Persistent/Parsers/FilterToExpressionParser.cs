using System;
using System.Linq.Expressions;

using Framework.CustomReports.Services.Persistent;
using Framework.QueryLanguage;

using Expression = System.Linq.Expressions.Expression;

namespace Framework.CustomReports.Services
{
    internal static class FilterToExpressionParser
    {
        internal static QueryLanguage.Expression Parse(PropertyExpression propertyExpression, EvaluatedFilter reportFilter, QueryLanguage.ConstantExpression constantExpression)
        {
            return TryToPropertyCall(propertyExpression, reportFilter.FilterOperator, constantExpression)
                   ?? TryBuildNullablePropertyCall(propertyExpression, reportFilter, constantExpression)
                   ?? ToMethodCall(propertyExpression, reportFilter.FilterOperator, constantExpression);
        }

        internal static Expression Parse(MemberExpression memberExpression, EvaluatedFilter reportFilter)
        {
            return TryToPropertyCall(memberExpression, reportFilter.FilterOperator, reportFilter.Value)
                   ?? TryBuildNullablePropertyCall(memberExpression, reportFilter.FilterOperator, reportFilter.Value)
                   ?? ToMethodCall(memberExpression, reportFilter.FilterOperator, reportFilter.Value);
        }

        private static QueryLanguage.Expression ToMethodCall(PropertyExpression source, string operation, QueryLanguage.ConstantExpression constValue)
        {
            return new MethodExpression(source, operation.ToMethodExpressionType(), constValue);
        }

        private static QueryLanguage.Expression TryToPropertyCall(PropertyExpression source, string binaryOperator, QueryLanguage.ConstantExpression constValue)
        {
            if (BinaryOperationTypeHelper.AllowOperations.TryGetValue(binaryOperator, out var expressionType))
            {
                return new QueryLanguage.BinaryExpression(source, expressionType, constValue);
            }

            return null;
        }

        private static QueryLanguage.Expression TryBuildNullablePropertyCall(PropertyExpression source, EvaluatedFilter filter, QueryLanguage.ConstantExpression constValue)
        {
            if (BinaryOperationTypeHelper.AllowNullableOperations.TryGetValue(filter.FilterOperator, out var expressionType))
            {
                var isNullExpression = new QueryLanguage.BinaryExpression(source, BinaryOperation.Equal, NullConstantExpression.Value);

                var expression = new QueryLanguage.BinaryExpression(source, expressionType, constValue);

                return new QueryLanguage.BinaryExpression(expression, BinaryOperation.OrElse, isNullExpression);
            }

            return null;
        }

        private static Expression ToMethodCall(MemberExpression source, string operation, string value)
        {
            return Expression.Call(source, operation, new Type[0], value.ToConstExprByType(source));
        }

        private static Expression TryToPropertyCall(MemberExpression source, string binaryOperator, string value)
        {
            var expressionType = AllowOperationsMapping.TryGetValue(binaryOperator);

            if (expressionType.HasValue)
            {
                var nextSource = source.TryIdentInject();

                return Expression.MakeBinary(expressionType.Value, nextSource, value.ToConstExprByType(nextSource));
            }

            return null;
        }

        private static Expression TryBuildNullablePropertyCall(MemberExpression source, string binaryOperator, string value)
        {
            if (AllowOperationsMapping.AllowNullableOperations.TryGetValue(binaryOperator, out var expressionType))
            {
                var nextSource = source.TryIdentInject();

                var sourceExpression = Expression.Convert(source, typeof(object));

                var isNullExpression = Expression.Equal(sourceExpression, Expression.Constant(null));

                var expression = Expression.MakeBinary(
                    expressionType,
                    nextSource,
                    value.ToConstExprByType(nextSource));

                return Expression.OrElse(expression, isNullExpression);
            }

            return null;
        }
    }
}
