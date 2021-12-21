using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Framework.Core
{
    public class OverrideCallInterfacePropertyVisitor : ExpressionVisitor
    {
        private readonly PropertyInfo _property;

        private readonly bool _isGeneric;


        public OverrideCallInterfacePropertyVisitor(PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            this._property = property;
            this._isGeneric = property.ReflectedType.IsGenericTypeDefinition;
        }


        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member is PropertyInfo && node.Member.Name == this._property.Name)
            {
                var property = node.Member as PropertyInfo;

                var overriding = this._isGeneric ? node.Expression.Type.IsGenericType
                                                   && node.Expression.Type.GetGenericTypeDefinition() == this._property.ReflectedType
                    : property == this._property;

                if (overriding)
                {
                    var expr = node.Expression is UnaryExpression // Convert Interface -> DomainObject?
                        ? (node.Expression as UnaryExpression).Operand
                        : node.Expression;

                    return Expression.Property(expr, expr.Type.GetImplementedProperty(this._property));
                }
            }

            return base.VisitMember(node);
        }
    }
}