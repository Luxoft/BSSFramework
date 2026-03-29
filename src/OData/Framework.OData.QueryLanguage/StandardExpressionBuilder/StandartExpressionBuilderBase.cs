using Framework.OData.QueryLanguage.Constant.Base;

using SExpressions = System.Linq.Expressions;

namespace Framework.OData.QueryLanguage.StandardExpressionBuilder;

public abstract class StandardExpressionBuilderBase : IStandardExpressionBuilder
{
    protected virtual SExpressions.Expression ToStandardExpressionBase(Expression expression, Dictionary<ParameterExpression, SExpressions.ParameterExpression> parameters, Type? expectedResultType = null)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));
        if (parameters == null) throw new ArgumentNullException(nameof(parameters));

        if (expression is LambdaExpression lambdaExpression)
        {
            return this.ToStandardExpression(lambdaExpression, parameters, expectedResultType);
        }

        if (expectedResultType != null)
        {
            throw new ArgumentOutOfRangeException(nameof(expectedResultType));
        }

        return expression switch
        {
            BinaryExpression binaryExpression => this.ToStandardExpression(binaryExpression, parameters),
            ParameterExpression parameterExpression => this.ToStandardExpression(parameterExpression, parameters),
            ConstantExpression constantExpression => this.ToStandardExpression(constantExpression),
            PropertyExpression propertyExpression => this.ToStandardExpression(propertyExpression, parameters),
            UnaryExpression unaryExpression => this.ToStandardExpression(unaryExpression, parameters),
            MethodExpression methodExpression => this.ToStandardExpression(methodExpression, parameters),
            _ => throw new ArgumentOutOfRangeException(nameof(expression))
        };
    }


    protected abstract SExpressions.LambdaExpression ToStandardExpression(LambdaExpression expression, Dictionary<ParameterExpression, SExpressions.ParameterExpression> parameters, Type? expectedResultType);

    protected abstract SExpressions.BinaryExpression ToStandardExpression(BinaryExpression expression, Dictionary<ParameterExpression, SExpressions.ParameterExpression> parameters);

    protected abstract SExpressions.MethodCallExpression ToStandardExpression(MethodExpression expression, Dictionary <ParameterExpression, System.Linq.Expressions.ParameterExpression> parameters);

    protected abstract SExpressions.ParameterExpression ToStandardExpression(ParameterExpression expression, Dictionary<ParameterExpression, System.Linq.Expressions.ParameterExpression> parameters);

    protected abstract SExpressions.ConstantExpression ToStandardExpression(ConstantExpression expression);

    protected abstract SExpressions.MemberExpression ToStandardExpression(PropertyExpression expression, Dictionary<ParameterExpression, System.Linq.Expressions.ParameterExpression> parameters);

    protected abstract SExpressions.UnaryExpression ToStandardExpression(UnaryExpression expression, Dictionary<ParameterExpression, System.Linq.Expressions.ParameterExpression> parameters);

    public SExpressions.Expression<TDelegate> ToStandardExpression<TDelegate>(LambdaExpression expression) => (SExpressions.Expression<TDelegate>)this.ToStandardExpression(expression, new Dictionary<ParameterExpression, SExpressions.ParameterExpression>(), typeof(TDelegate));
}
