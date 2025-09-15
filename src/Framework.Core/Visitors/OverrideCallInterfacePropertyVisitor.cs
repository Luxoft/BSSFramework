using System.Linq.Expressions;
using System.Reflection;

namespace Framework.Core.Visitors;

public class OverrideCallInterfacePropertyVisitor : ExpressionVisitor
{
    private readonly PropertyInfo property;

    private readonly bool isGeneric;


    public OverrideCallInterfacePropertyVisitor(PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        this.property = property;
        this.isGeneric = property.ReflectedType!.IsGenericTypeDefinition;
    }


    protected override Expression VisitMember(MemberExpression node)
    {
        if (node.Member is PropertyInfo && node.Member.Name == this.property.Name)
        {
            var property = node.Member as PropertyInfo;

            var overriding = this.isGeneric ? node.Expression.Type.IsGenericType
                                               && node.Expression.Type.GetGenericTypeDefinition() == this.property.ReflectedType
                                     : property == this.property;

            if (overriding)
            {
                var expr = node.Expression is UnaryExpression // Convert Interface -> DomainObject?
                                   ? (node.Expression as UnaryExpression)!.Operand
                                   : node.Expression;

                return Expression.Property(expr, expr.Type.GetImplementedProperty(property));
            }
        }

        return base.VisitMember(node);
    }
}
