using System;
using System.Linq.Expressions;

namespace Framework.Core
{
    public interface IExpressionParser
    {
        Type ExpressionType { get; }

        string ExpectedFormat { get; }
    }

    public interface IExpressionParser<in TSource, out TDelegate, out TExpression> : IExpressionParser
        where TExpression : LambdaExpression
    {
        TDelegate GetDelegate(TSource source, bool wrapError = true);

        TExpression GetExpression(TSource source, bool wrapError = true);

        void Validate(TSource source);
    }

    public interface IExpressionParser<in TSource, TDelegate> : IExpressionParser<TSource, TDelegate, Expression<TDelegate>>
        where TDelegate : class
    {

    }
}