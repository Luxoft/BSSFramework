using CommonFramework;

using Framework.Core;

using SExpressions = System.Linq.Expressions;

namespace Framework.QueryLanguage;

public class StandardExpressionBuilder : StandardExpressionBuilderBase
{
    protected override SExpressions.LambdaExpression ToStandardExpression(LambdaExpression expression, Dictionary<ParameterExpression, SExpressions.ParameterExpression> parameters, Type expectedResultType)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));
        if (parameters == null) throw new ArgumentNullException(nameof(parameters));
        if (expectedResultType == null) throw new ArgumentNullException(nameof(expectedResultType));


        var parameterPairs = expression.Parameters.ZipStrong(expectedResultType.GetGenericArguments().SkipLast(1), (parameter, type) =>
                                                                     new { InParameter = parameter, OutParameter = SExpressions.Expression.Parameter(type, parameter.Name) }).ToList();

        var sparameters = parameterPairs.ToDictionary(pair => pair.InParameter, pair => pair.OutParameter);

        var summParameters = sparameters.Concat(parameters);

        var bodyExpr = this.ToStandardExpressionBase(expression.Body, summParameters);

        return SExpressions.Expression.Lambda(expectedResultType, bodyExpr, parameterPairs.Select(pair => pair.OutParameter));
    }

    protected override SExpressions.BinaryExpression ToStandardExpression(BinaryExpression expression, Dictionary<ParameterExpression, SExpressions.ParameterExpression> parameters)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));
        if (parameters == null) throw new ArgumentNullException(nameof(parameters));

        var preLeftStandardExpression = this.ToStandardExpressionBase(expression.Left, parameters);
        var preRightStandardExpression = this.ToStandardExpressionBase(expression.Right, parameters);

        var leftStandardExpression = preLeftStandardExpression.TryNormalize(preLeftStandardExpression.Type, preRightStandardExpression.Type);

        var rightStandardExpression = preRightStandardExpression.TryNormalize(preLeftStandardExpression.Type, preRightStandardExpression.Type);


        return SExpressions.Expression.MakeBinary(expression.Operation.ToExpressionType(), leftStandardExpression, rightStandardExpression);
    }


    protected override SExpressions.MethodCallExpression ToStandardExpression(MethodExpression expression, Dictionary<ParameterExpression, System.Linq.Expressions.ParameterExpression> parameters)
    {
        var standardSourceExpr = this.ToStandardExpressionBase(expression.Source, parameters);

        var elementType = standardSourceExpr.Type.GetCollectionElementType();

        var delegateType = elementType.Maybe(v => typeof(Func<,>).MakeGenericType(v, typeof(bool)));

        var standardArguments = expression.Arguments.Select(arg => this.ToStandardExpressionBase(arg, parameters, delegateType)).ToList();

        var allArgs = new[] { standardSourceExpr }.Concat(standardArguments).ToList();

        return expression.Type switch
        {
            MethodExpressionType.StringStartsWith => SExpressions.Expression.Call(
                this.TryCallToString(standardSourceExpr),
                MethodInfoHelper.StringStartsWithMethod,
                standardArguments),
            MethodExpressionType.StringContains => SExpressions.Expression.Call(this.TryCallToString(standardSourceExpr), MethodInfoHelper.StringContainsMethod, standardArguments),
            MethodExpressionType.StringEndsWith => SExpressions.Expression.Call(this.TryCallToString(standardSourceExpr), MethodInfoHelper.StringEndsWithMethod, standardArguments),
            MethodExpressionType.CollectionAny => SExpressions.Expression.Call(
                standardArguments.Any()
                    ? MethodInfoHelper.CollectionAnyFilterMethod.MakeGenericMethod(elementType)
                    : MethodInfoHelper.CollectionAnyEmptyMethod.MakeGenericMethod(elementType),
                allArgs),
            MethodExpressionType.CollectionAll => SExpressions.Expression.Call(MethodInfoHelper.CollectionAllFilterMethod.MakeGenericMethod(elementType), allArgs),
            _ => throw new ArgumentOutOfRangeException("this.Type")
        };
    }

    protected override SExpressions.ParameterExpression ToStandardExpression(ParameterExpression expression, Dictionary<ParameterExpression, System.Linq.Expressions.ParameterExpression> parameters) => parameters[expression];

    protected override SExpressions.ConstantExpression ToStandardExpression(ConstantExpression expression) => SExpressions.Expression.Constant(expression.GetRawValue());

    protected override SExpressions.MemberExpression ToStandardExpression(PropertyExpression expression, Dictionary<ParameterExpression, System.Linq.Expressions.ParameterExpression> parameters)
    {
        var preSourceExpr = this.ToStandardExpressionBase(expression.Source, parameters);

        var sourceExpr = preSourceExpr.Type.IsNullable() ? SExpressions.Expression.Property(preSourceExpr, "Value") : preSourceExpr;

        return CoreExpressionHelper.PropertyOrFieldAuto(sourceExpr, expression.PropertyName);
    }

    protected override SExpressions.UnaryExpression ToStandardExpression(UnaryExpression expression, Dictionary<ParameterExpression, System.Linq.Expressions.ParameterExpression> parameters)
    {
        var standardOperand = this.ToStandardExpressionBase(expression.Operand, parameters);

        return SExpressions.Expression.MakeUnary(expression.Operation.ToExpressionType(), standardOperand, standardOperand.Type);
    }

    private SExpressions.Expression TryCallToString(SExpressions.Expression source)
    {
        var memberExpression = source as SExpressions.MemberExpression;
        if (null == memberExpression)
        {
            return source;
        }

        if (memberExpression.Type != typeof(string))
        {
            return SExpressions.Expression.Call(memberExpression, nameof(string.ToString), []);
        }

        return source;
    }

    public static readonly StandardExpressionBuilder Default = new StandardExpressionBuilder();
}
