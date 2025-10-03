using System.Linq.Expressions;
using System.Reflection;

using CommonFramework;

namespace Framework.Core;

public static class PropertyPathBuilder
{
    public static Expression<Action<IPropertyPathNode<TSource>>> Build<TSource>(Expression<Action<IPropertyPathNode<TSource>>> path)
    {
        return path;
    }

    //public static Expression<Action<IPropertyPathNode<TSource, TSelect>>> Build<TSource, TSelect>(Expression<Action<IPropertyPathNode<TSource, TSelect>>> path)
    //{
    //    return path;
    //}


    public static Expression<Action<IPropertyPathNode<TSource>>> ToNode<TSource>(this PropertyPath path)
    {
        var ruleParameter = Expression.Parameter(typeof(IPropertyPathNode<TSource>), "rule");

        var totalPath = path.Aggregate((Expression)ruleParameter, (expr, property) =>
                                                                  {
                                                                      var selectType = expr.Type.GetGenericArguments().Single();

                                                                      var selectParam = Expression.Parameter(selectType, selectType.Name.ToStartLowerCase());

                                                                      var propExpr = Expression.Property(selectParam, property);

                                                                      var methodCallInfo = property.PropertyType.IsCollection()

                                                                              ? property.PropertyType.GetCollectionElementType().Pipe(elementType => new

                                                                                  {
                                                                                          Name = "SelectMany",
                                                                                          ElementType = elementType,
                                                                                          ReturnType = typeof(IEnumerable<>).MakeGenericType(elementType)
                                                                                  })

                                                                              : new
                                                                                {
                                                                                        Name = "SelectNested",
                                                                                        ElementType = property.PropertyType,
                                                                                        ReturnType = property.PropertyType
                                                                                };


                                                                      var selectLambda = Expression.Lambda(typeof(Func<,>).MakeGenericType(selectParam.Type, methodCallInfo.ReturnType), propExpr, selectParam);

                                                                      try
                                                                      {
                                                                          return (Expression)Expression.Call(expr, methodCallInfo.Name, new[] { methodCallInfo.ElementType }, selectLambda);
                                                                      }
                                                                      catch (Exception ex)
                                                                      {
                                                                          throw new Exception($"Can't select property \"{property.Name}\" with element Type \"{methodCallInfo.ElementType.Name}\"", ex);
                                                                      }
                                                                  });

        return Expression.Lambda<Action<IPropertyPathNode<TSource>>>(totalPath, ruleParameter);
    }



    public static IEnumerable<PropertyPath> ToPropertyPaths<TSource>(this Expression<Action<IPropertyPathNode<TSource>>> expression)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));

        var properties = expression.Parameters.Single().GetPropertyPaths(expression.Body);

        return properties.Select(path => path.ToPropertyPath());
    }

    private static IEnumerable<IEnumerable<PropertyInfo>> GetPropertyPaths(this Expression ruleParameter, Expression selectState)
    {
        if (ruleParameter == null) throw new ArgumentNullException(nameof(ruleParameter));
        if (selectState == null) throw new ArgumentNullException(nameof(selectState));


        return ruleParameter.GetProperties(selectState).GetEnumerator().GetPropertyPaths(new PropertyInfo[0], false);
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
