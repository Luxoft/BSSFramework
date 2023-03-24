using System;
using System.Collections.Generic;

using SExpressions = System.Linq.Expressions;

namespace Framework.QueryLanguage;

public abstract class StandartExpressionBuilderBase : IStandartExpressionBuilder
{
    protected virtual SExpressions.Expression ToStandartExpressionBase(Expression expression, Dictionary<ParameterExpression, SExpressions.ParameterExpression> parameters, Type expectedResultType = null)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));
        if (parameters == null) throw new ArgumentNullException(nameof(parameters));

        if (expression is LambdaExpression)
        {
            return this.ToStandartExpression(expression as LambdaExpression, parameters, expectedResultType);
        }

        if (expectedResultType != null)
        {
            throw new ArgumentOutOfRangeException(nameof(expectedResultType));
        }

        if (expression is BinaryExpression)
        {
            return this.ToStandartExpression(expression as BinaryExpression, parameters);
        }
        else if (expression is ParameterExpression)
        {
            return this.ToStandartExpression(expression as ParameterExpression, parameters);
        }
        else if (expression is ConstantExpression)
        {
            return this.ToStandartExpression(expression as ConstantExpression);
        }
        else if (expression is PropertyExpression)
        {
            return this.ToStandartExpression(expression as PropertyExpression, parameters);
        }
        else if (expression is UnaryExpression)
        {
            return this.ToStandartExpression(expression as UnaryExpression, parameters);
        }
        else if (expression is MethodExpression)
        {
            return this.ToStandartExpression(expression as MethodExpression, parameters);
        }
        else if (expression is ExpandContainsExpression)
        {
            return this.ToStandartExpression(expression as ExpandContainsExpression, parameters);
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(expression));
        }
    }


    protected abstract SExpressions.LambdaExpression ToStandartExpression(LambdaExpression expression, Dictionary<ParameterExpression, SExpressions.ParameterExpression> parameters, Type expectedResultType);

    protected abstract SExpressions.BinaryExpression ToStandartExpression(BinaryExpression expression, Dictionary<ParameterExpression, SExpressions.ParameterExpression> parameters);

    protected abstract SExpressions.MethodCallExpression ToStandartExpression(MethodExpression expression, Dictionary <ParameterExpression, System.Linq.Expressions.ParameterExpression> parameters);

    protected abstract SExpressions.ParameterExpression ToStandartExpression(ParameterExpression expression, Dictionary<ParameterExpression, System.Linq.Expressions.ParameterExpression> parameters);

    protected abstract SExpressions.ConstantExpression ToStandartExpression(ConstantExpression expression);

    protected abstract SExpressions.MemberExpression ToStandartExpression(PropertyExpression expression, Dictionary<ParameterExpression, System.Linq.Expressions.ParameterExpression> parameters);

    protected abstract SExpressions.UnaryExpression ToStandartExpression(UnaryExpression expression, Dictionary<ParameterExpression, System.Linq.Expressions.ParameterExpression> parameters);

    protected abstract SExpressions.Expression ToStandartExpression(ExpandContainsExpression expression, Dictionary<ParameterExpression, System.Linq.Expressions.ParameterExpression> parameters);


    public SExpressions.Expression<TDelegate> ToStandartExpression<TDelegate>(LambdaExpression expression)
    {
        return (SExpressions.Expression<TDelegate>)this.ToStandartExpression(expression, new Dictionary<ParameterExpression, SExpressions.ParameterExpression>(), typeof(TDelegate));
    }
}
