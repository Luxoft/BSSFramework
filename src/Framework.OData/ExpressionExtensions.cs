using CommonFramework;

using Framework.QueryLanguage;

namespace Framework.OData;

public static class ExpressionExtensions
{
    public static IEnumerable<Tuple<string, string>> GetPropertyPath(this LambdaExpression expression)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));

        var startParam = expression.Parameters.Single();

        var startProp = ((PropertyExpression)expression.Body);

        var path = startProp.GetAllElements(prop => (prop.Source as ParameterExpression).Maybe(s => s.Name == startParam.Name)
                                                        ? null
                                                        : ((PropertyExpression)prop.Source));

        return path.Reverse().Select(expr => Tuple.Create(expr.PropertyName, (expr as SelectExpression).Maybe(selectExpr => selectExpr.Alias)));
    }
}
