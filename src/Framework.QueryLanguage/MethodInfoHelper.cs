using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Framework.Core;
using Framework.HierarchicalExpand;

namespace Framework.QueryLanguage
{
    public static class MethodInfoHelper
    {
        public static readonly MethodInfo StringContainsMethod = new Func<string, bool>("".Contains).Method;

        public static readonly MethodInfo StringStartsWithMethod = new Func<string, bool>("".StartsWith).Method;

        public static readonly MethodInfo StringEndsWithMethod = new Func<string, bool>("".EndsWith).Method;

        public static readonly MethodInfo PeriodContainsMethod = new Func<Period, DateTime, bool>(CommonPeriodExtensions.Contains).Method;

        public static readonly MethodInfo NullablePeriodContainsMethod = new Func<Period?, DateTime?, bool>(CommonPeriodExtensions.Contains).Method;

        public static readonly MethodInfo PeriodIsIntersectedMethod = new Func<Period, Period, bool>(PeriodExtensions.IsIntersected).Method;

        public static readonly MethodInfo NullablePeriodIsIntersectedMethod = new Func<Period?, Period?, bool>(PeriodExtensions.IsIntersected).Method;

        public static readonly MethodInfo CollectionAnyEmptyMethod = new Func<IEnumerable<int>, bool>(Enumerable.Any).Method.GetGenericMethodDefinition();

        public static readonly MethodInfo CollectionAnyFilterMethod = new Func<IEnumerable<int>, Func<int, bool>, bool>(Enumerable.Any).Method.GetGenericMethodDefinition();

        public static readonly MethodInfo CollectionAllFilterMethod = new Func<IEnumerable<int>, Func<int, bool>, bool>(Enumerable.All).Method.GetGenericMethodDefinition();

        public static readonly MethodInfo ExpandContainsMethod = typeof(HierarchicalExtensions).GetMethod(nameof(HierarchicalExtensions.IsExpandableBy));

        public static MethodExpressionType? GetMethodType(this MethodInfo methodInfo)
        {
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));

            if (methodInfo == StringStartsWithMethod)
            {
                return MethodExpressionType.StringStartsWith;
            }

            if (methodInfo == StringContainsMethod)
            {
                return MethodExpressionType.StringContains;
            }

            if (methodInfo == StringEndsWithMethod)
            {
                return MethodExpressionType.StringEndsWith;
            }

            if (methodInfo == PeriodContainsMethod)
            {
                return MethodExpressionType.PeriodContains;
            }

            if (methodInfo == PeriodIsIntersectedMethod || methodInfo == NullablePeriodIsIntersectedMethod)
            {
                return MethodExpressionType.PeriodIsIntersected;
            }

            if (methodInfo.IsGenericMethod)
            {
                var genMethodInfo = methodInfo.GetGenericMethodDefinition();

                if (genMethodInfo == CollectionAnyEmptyMethod || genMethodInfo == CollectionAnyFilterMethod)
                {
                    return MethodExpressionType.CollectionAny;
                }

                if (genMethodInfo == CollectionAllFilterMethod)
                {
                    return MethodExpressionType.CollectionAll;
                }
            }

            return null;
        }
    }
}
