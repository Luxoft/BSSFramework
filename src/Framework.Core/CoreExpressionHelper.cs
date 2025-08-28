using System.Linq.Expressions;

namespace Framework.Core;

public static class CoreExpressionHelper
{
    public static MemberExpression PropertyOrFieldAuto(Expression expr, string memberName)
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

    public static Expression<Func<T1, T2, TResult>> Create<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> func)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        return func;
    }
}
