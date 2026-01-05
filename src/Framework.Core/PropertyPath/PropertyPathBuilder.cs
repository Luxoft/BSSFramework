using System.Linq.Expressions;
using System.Reflection;

namespace Framework.Core;

public static class PropertyPathBuilder
{
    private static IEnumerable<IEnumerable<PropertyInfo>> GetPropertyPaths(this Expression ruleParameter, Expression selectState)
    {
        if (ruleParameter == null) throw new ArgumentNullException(nameof(ruleParameter));
        if (selectState == null) throw new ArgumentNullException(nameof(selectState));


        return ruleParameter.GetProperties(selectState).GetEnumerator().GetPropertyPaths([], false);
    }

    private static IEnumerable<PropertyInfo[]> GetPropertyPaths(this IEnumerator<KeyValuePair<MethodInfo, PropertyInfo>> selectPathEnumerator, PropertyInfo[] prevPath, bool lastIsSelect)
    {
        if (selectPathEnumerator == null) throw new ArgumentNullException(nameof(selectPathEnumerator));
        if (prevPath == null) throw new ArgumentNullException(nameof(prevPath));

        if (selectPathEnumerator.MoveNext())
        {
            var pair = selectPathEnumerator.Current;

            var nextPath = prevPath.Concat(new[] { pair.Value }).ToArray();

            switch (pair.Key.Name)
            {
                case "SelectNested":
                case "SelectMany":
                {
                    foreach (var path in selectPathEnumerator.GetPropertyPaths(nextPath, false))
                    {
                        yield return path;
                    }
                }
                    break;

                case "Select":
                {
                    yield return nextPath;

                    foreach (var path in selectPathEnumerator.GetPropertyPaths(prevPath, true))
                    {
                        yield return path;
                    }
                }
                    break;

                default:
                    throw new ArgumentOutOfRangeException("pair.Key.Name");
            }
        }
        else if (!lastIsSelect)
        {
            yield return prevPath;
        }
    }

    private static IEnumerable<KeyValuePair<MethodInfo, PropertyInfo>> GetProperties(this Expression ruleParameter, Expression selectState)
    {
        if (ruleParameter == null) throw new ArgumentNullException(nameof(ruleParameter));
        if (selectState == null) throw new ArgumentNullException(nameof(selectState));

        if (selectState == ruleParameter)
        {
            yield break;
        }
        else
        {
            var callExpression = (MethodCallExpression)selectState;

            foreach (var next in ruleParameter.GetProperties(callExpression.Object))
            {
                yield return next;
            }

            var propLambda = (LambdaExpression)callExpression.Arguments.Single();

            var member = (MemberExpression)propLambda.Body;

            yield return new (callExpression.Method, (PropertyInfo)member.Member);
        }
    }
}
