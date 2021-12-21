using System;
using System.Linq.Expressions;

namespace Framework.CustomReports.Services
{
    public class IgnoreCaseStringExpressionVisitor : ExpressionVisitor
    {
        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.Type == typeof(string))
            {
                return Expression.Constant((((string) node.Value) ?? "").ToLower());
            }

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member.MemberType == System.Reflection.MemberTypes.Property)
            {
                return Expression.Call(node, typeof(string).GetMethod("ToLower", new Type[] { }));
            }
            return base.VisitMember(node);
        }
    }
}