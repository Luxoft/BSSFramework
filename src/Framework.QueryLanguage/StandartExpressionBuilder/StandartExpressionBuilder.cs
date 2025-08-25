using CommonFramework;

using Framework.Core;

using ExpressionHelper = Framework.Core.ExpressionHelper;
using SExpressions = System.Linq.Expressions;

namespace Framework.QueryLanguage;

public class StandartExpressionBuilder : StandartExpressionBuilderBase
{
    protected override SExpressions.LambdaExpression ToStandartExpression(LambdaExpression expression, Dictionary<ParameterExpression, SExpressions.ParameterExpression> parameters, Type expectedResultType)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));
        if (parameters == null) throw new ArgumentNullException(nameof(parameters));
        if (expectedResultType == null) throw new ArgumentNullException(nameof(expectedResultType));


        var parameterPairs = expression.Parameters.ZipStrong(expectedResultType.GetGenericArguments().SkipLast(1), (parameter, type) =>
                                                                     new { InParameter = parameter, OutParameter = SExpressions.Expression.Parameter(type, parameter.Name) }).ToList();

        var sparameters = parameterPairs.ToDictionary(pair => pair.InParameter, pair => pair.OutParameter);

        var summParameters = sparameters.Concat(parameters);

        var bodyExpr = this.ToStandartExpressionBase(expression.Body, summParameters);

        return SExpressions.Expression.Lambda(expectedResultType, bodyExpr, parameterPairs.Select(pair => pair.OutParameter));
    }

    protected override SExpressions.BinaryExpression ToStandartExpression(BinaryExpression expression, Dictionary<ParameterExpression, SExpressions.ParameterExpression> parameters)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));
        if (parameters == null) throw new ArgumentNullException(nameof(parameters));

        var preLeftStandartExpression = this.ToStandartExpressionBase(expression.Left, parameters);
        var preRightStandartExpression = this.ToStandartExpressionBase(expression.Right, parameters);

        var leftStandartExpression = preLeftStandartExpression.TryNormalize(preLeftStandartExpression.Type, preRightStandartExpression.Type);

        var rightStandartExpression = preRightStandartExpression.TryNormalize(preLeftStandartExpression.Type, preRightStandartExpression.Type);


        return SExpressions.Expression.MakeBinary(expression.Operation.ToExpressionType(), leftStandartExpression, rightStandartExpression);
    }


    protected override SExpressions.MethodCallExpression ToStandartExpression(MethodExpression expression, Dictionary<ParameterExpression, System.Linq.Expressions.ParameterExpression> parameters)
    {
        var standartSourceExpr = this.ToStandartExpressionBase(expression.Source, parameters);

        var elementType = standartSourceExpr.Type.GetCollectionElementType();

        var delegateType = elementType.Maybe(v => typeof(Func<,>).MakeGenericType(v, typeof(bool)));

        var standartArguments = expression.Arguments.Select(arg => this.ToStandartExpressionBase(arg, parameters, delegateType)).ToList();

        var allArgs = new[] { standartSourceExpr }.Concat(standartArguments).ToList();

        switch (expression.Type)
        {
            case MethodExpressionType.StringStartsWith:
                return SExpressions.Expression.Call(this.TryCallToString(standartSourceExpr), MethodInfoHelper.StringStartsWithMethod, standartArguments);

            case MethodExpressionType.StringContains:
                return SExpressions.Expression.Call(this.TryCallToString(standartSourceExpr), MethodInfoHelper.StringContainsMethod, standartArguments);

            case MethodExpressionType.StringEndsWith:
                return SExpressions.Expression.Call(this.TryCallToString(standartSourceExpr), MethodInfoHelper.StringEndsWithMethod, standartArguments);

            case MethodExpressionType.CollectionAny:
                return SExpressions.Expression.Call(standartArguments.Any() ? MethodInfoHelper.CollectionAnyFilterMethod.MakeGenericMethod(elementType) : MethodInfoHelper.CollectionAnyEmptyMethod.MakeGenericMethod(elementType), allArgs);

            case MethodExpressionType.CollectionAll:
                return SExpressions.Expression.Call(MethodInfoHelper.CollectionAllFilterMethod.MakeGenericMethod(elementType), allArgs);

            default:
                throw new ArgumentOutOfRangeException("this.Type");
        }
    }

    protected override SExpressions.ParameterExpression ToStandartExpression(ParameterExpression expression, Dictionary<ParameterExpression, System.Linq.Expressions.ParameterExpression> parameters)
    {
        return parameters[expression];
    }

    protected override SExpressions.ConstantExpression ToStandartExpression(ConstantExpression expression)
    {
        return SExpressions.Expression.Constant(expression.UntypedValue);
    }


    protected override SExpressions.MemberExpression ToStandartExpression(PropertyExpression expression, Dictionary<ParameterExpression, System.Linq.Expressions.ParameterExpression> parameters)
    {
        var preSourceExpr = this.ToStandartExpressionBase(expression.Source, parameters);

        var sourceExpr = preSourceExpr.Type.IsNullable() ? SExpressions.Expression.Property(preSourceExpr, "Value") : preSourceExpr;

        return ExpressionHelper.PropertyOrFieldAuto(sourceExpr, expression.PropertyName);
    }

    protected override SExpressions.UnaryExpression ToStandartExpression(UnaryExpression expression, Dictionary<ParameterExpression, System.Linq.Expressions.ParameterExpression> parameters)
    {
        var standartOperand = this.ToStandartExpressionBase(expression.Operand, parameters);

        return SExpressions.Expression.MakeUnary(expression.Operation.ToExpressionType(), standartOperand, standartOperand.Type);
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
            return SExpressions.Expression.Call(memberExpression, nameof(string.ToString), new Type[0] { });
        }

        return source;
    }

    public static readonly StandartExpressionBuilder Default = new StandartExpressionBuilder();
}
