using SExpressions = System.Linq.Expressions;

namespace Framework.QueryLanguage;

public abstract class StandardExpressionBuilderBase : IStandardExpressionBuilder
{
    protected virtual SExpressions.Expression ToStandardExpressionBase(Expression expression, Dictionary<ParameterExpression, SExpressions.ParameterExpression> parameters, Type expectedResultType = null)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));
        if (parameters == null) throw new ArgumentNullException(nameof(parameters));

        if (expression is LambdaExpression)
        {
            return this.ToStandardExpression(expression as LambdaExpression, parameters, expectedResultType);
        }

        if (expectedResultType != null)
        {
            throw new ArgumentOutOfRangeException(nameof(expectedResultType));
        }

        if (expression is BinaryExpression)
        {
            return this.ToStandardExpression(expression as BinaryExpression, parameters);
        }
        else if (expression is ParameterExpression)
        {
            return this.ToStandardExpression(expression as ParameterExpression, parameters);
        }
        else if (expression is ConstantExpression)
        {
            return this.ToStandardExpression(expression as ConstantExpression);
        }
        else if (expression is PropertyExpression)
        {
            return this.ToStandardExpression(expression as PropertyExpression, parameters);
        }
        else if (expression is UnaryExpression)
        {
            return this.ToStandardExpression(expression as UnaryExpression, parameters);
        }
        else if (expression is MethodExpression)
        {
            return this.ToStandardExpression(expression as MethodExpression, parameters);
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(expression));
        }
    }


    protected abstract SExpressions.LambdaExpression ToStandardExpression(LambdaExpression expression, Dictionary<ParameterExpression, SExpressions.ParameterExpression> parameters, Type expectedResultType);

    protected abstract SExpressions.BinaryExpression ToStandardExpression(BinaryExpression expression, Dictionary<ParameterExpression, SExpressions.ParameterExpression> parameters);

    protected abstract SExpressions.MethodCallExpression ToStandardExpression(MethodExpression expression, Dictionary <ParameterExpression, System.Linq.Expressions.ParameterExpression> parameters);

    protected abstract SExpressions.ParameterExpression ToStandardExpression(ParameterExpression expression, Dictionary<ParameterExpression, System.Linq.Expressions.ParameterExpression> parameters);

    protected abstract SExpressions.ConstantExpression ToStandardExpression(ConstantExpression expression);

    protected abstract SExpressions.MemberExpression ToStandardExpression(PropertyExpression expression, Dictionary<ParameterExpression, System.Linq.Expressions.ParameterExpression> parameters);

    protected abstract SExpressions.UnaryExpression ToStandardExpression(UnaryExpression expression, Dictionary<ParameterExpression, System.Linq.Expressions.ParameterExpression> parameters);

    public SExpressions.Expression<TDelegate> ToStandardExpression<TDelegate>(LambdaExpression expression) => (SExpressions.Expression<TDelegate>)this.ToStandardExpression(expression, new Dictionary<ParameterExpression, SExpressions.ParameterExpression>(), typeof(TDelegate));
}
