using System;
using System.Linq.Expressions;

namespace Framework.Core
{
    public interface IExpressionConverter
    {
        LambdaExpression GetConvertExpressionBase();
    }

    public interface IExpressionConverter<TSource, TTarget> : IExpressionConverter
    {
        Expression<Func<TSource, TTarget>> GetConvertExpression();
    }
}