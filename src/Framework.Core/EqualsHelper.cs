using System;
using System.Linq.Expressions;

namespace Framework.Core;

public static class EqualsHelper<T>
{
    public static readonly Func<T, Expression<Func<T, bool>>> GetEqualsExpression = GetGetEqualsExpression();


    private static Func<T, Expression<Func<T, bool>>> GetGetEqualsExpression()
    {
        var param = Expression.Parameter(typeof(T));

        var innerParam = Expression.Parameter(typeof(T));

        var eqMethod = typeof(T).GetEqualityMethod(true);

        var equalsExpr = Expression.Equal(param, innerParam, typeof(T).IsNullable(), eqMethod);

        var innerLambda = Expression.Lambda(equalsExpr, innerParam);

        var expr = Expression.Lambda<Func<T, Expression<Func<T, bool>>>>(Expression.Quote(innerLambda), param);

        return expr.Compile();
    }
}
