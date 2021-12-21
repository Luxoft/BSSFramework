using System.Linq.Expressions;
using System.Reflection;

namespace Framework.Core
{
    /// <summary>
    /// Выправление MemberExpression со свойствами, которых ReflectedType не текущий, а базовый
    /// </summary>
    public class FixPropertySourceVisitor : ExpressionVisitor
    {
        public static readonly FixPropertySourceVisitor Value = new FixPropertySourceVisitor();


        private FixPropertySourceVisitor()
        {
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            var propRequest = from property in (node.Member as PropertyInfo).ToMaybe()

                          where node.Expression.Type != property.ReflectedType

                          let realProp = node.Expression.Type.GetProperty(property.Name, true)

                          select (Expression)Expression.Property(node.Expression, realProp);


            return propRequest.GetValueOrDefault(() => base.VisitMember(node));
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var methodRequest = from _ in Maybe.Return()

                                where node.Object.Type != node.Method.ReflectedType

                                let realMethod = node.Object.Type.GetMethod(node.Method.Name, true)

                                select (Expression)Expression.Call(node.Object, realMethod, node.Arguments);

            return methodRequest.GetValueOrDefault(() => base.VisitMethodCall(node));
        }
    }
}
