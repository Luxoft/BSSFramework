using System;
using System.Linq;
using System.Linq.Expressions;

namespace Framework.Core;

public static class ExpressionBuilder
{
    public static Expression<Action<TObject, TProperty>> BuildAction<TObject, TProperty>(string expressionString)
    {
        var path = expressionString;
        var tObjectParameterName = "master";
        var tObjectParameter = Expression.Parameter(typeof (TObject), tObjectParameterName);

        var tPropertyParameterName = "property";
        var tPropertyParameter = Expression.Parameter(typeof (TProperty), tPropertyParameterName);


        var pathValues = path.Split('.');

        //var expr1 = GetExpressionBy(pathValues.First(), tObjectParameter);
        //for (var q = 1; q < pathValues.Count(); q++)
        //{
        //    expr1 = GetExpressionBy(pathValues[q], expr1);
        //}



        Expression expr1 = pathValues.Aggregate((Expression) tObjectParameter,
                                                (current, currentPath) => GetExpressionBy(currentPath, current));


        return Expression.Lambda<Action<TObject, TProperty>>(
                                                             Expression.Assign
                                                                     (
                                                                      expr1,
                                                                      tPropertyParameter
                                                                     ),
                                                             tObjectParameter, tPropertyParameter);
    }

    private static Expression GetExpressionBy(string value, Expression prevExpression)
    {
        if (value.Contains("["))
        {
            var propertyName = new string(value.TakeWhile(z => z != '[').ToArray());

            if (!string.IsNullOrEmpty(propertyName))
            {
                //for collection

                var collectionPropertyExpression = Expression.Property(prevExpression, propertyName);
                var indexerParameters =
                        new string(value.SkipWhile(z => z != '[').Skip(1).TakeWhile(z => z != ']').ToArray());
                var splittedIndexerParameters = indexerParameters.Split(',');
                var splittedIndexExpressionParameters =
                        splittedIndexerParameters.Select(z => Expression.Constant(Convert.ToInt32(z), typeof(int)));

                return Expression.Call(collectionPropertyExpression, "get_Item", new Type[0],
                                       splittedIndexExpressionParameters.ToArray());
            }
            else
            {
                //for indexer

                var indexerParameters = new string(value.SkipWhile(z => z != '[').Skip(1).TakeWhile(z => z != ']').ToArray());
                var splittedIndexerParameters = indexerParameters.Split(',');
                var splittedIndexExpressionParameters =
                        splittedIndexerParameters.Select(z => Expression.Constant(z.ToString(), typeof(string)));

                return Expression.Call(prevExpression, "get_Item", new Type[0],
                                       splittedIndexExpressionParameters.ToArray());
            }
        }
        return Expression.Property(prevExpression, value);
    }

}
