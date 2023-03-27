using System.Linq.Expressions;

using JetBrains.Annotations;

namespace Framework.Core;

public static class ExpressionHelper
{
    public static Expression<Func<T, T>> GetIdentity<T>()
    {
        return x => x;
    }

    public static Expression<Func<T, T, bool>> GetEquality<T>()
    {
        var p1 = Expression.Parameter(typeof(T));
        var p2 = Expression.Parameter(typeof(T));

        return Expression.Lambda<Func<T, T, bool>>(Expression.Equal(p1, p2), p1, p2);
    }

    public static MemberExpression PropertyOrFieldAuto(Expression expr, [NotNull] string memberName)
    {
        if (expr == null) throw new ArgumentNullException(nameof(expr));
        if (memberName == null) throw new ArgumentNullException(nameof(memberName));

        if (expr.Type.IsInterface)
        {
            var property = expr.Type.GetAllInterfaceProperties().FirstOrDefault(prop => prop.Name == memberName);

            if (property != null)
            {
                return Expression.Property(expr, property);
            }
        }

        return Expression.PropertyOrField(expr, memberName);
    }

    public static Expression<Func<TResult>> Create<TResult>(Expression<Func<TResult>> func)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        return func;
    }

    public static Expression<Func<T, TResult>> Create<T, TResult>(Expression<Func<T, TResult>> func)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        return func;
    }

    public static Expression<Func<T1, T2, TResult>> Create<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> func)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        return func;
    }

    public static Expression<Func<T1, T2, T3, TResult>> Create<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> func)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        return func;
    }

    public static Expression<Func<T1, T2, T3, T4, TResult>> Create<T1, T2, T3, T4, TResult>(Expression<Func<T1, T2, T3, T4, TResult>> func)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        return func;
    }
}
