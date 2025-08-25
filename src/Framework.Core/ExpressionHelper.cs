using System.Linq.Expressions;

namespace Framework.Core;

public static class ExpressionHelper
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
}
