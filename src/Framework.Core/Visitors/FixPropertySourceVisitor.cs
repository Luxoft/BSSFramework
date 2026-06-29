using System.Linq.Expressions;
using System.Reflection;

using Anch.Core;

namespace Framework.Core.Visitors;

/// <summary>
/// Выправление MemberExpression со свойствами, которых ReflectedType не текущий, а базовый
/// </summary>
public class FixPropertySourceVisitor : ExpressionVisitor
{
    public static readonly FixPropertySourceVisitor Value = new();


    private FixPropertySourceVisitor()
    {
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        var propRequest = from property in (node.Member as PropertyInfo).ToMaybe()

                          from source in node.Expression.ToMaybe()

                          where source.Type != property.ReflectedType

                          let realProp = source.Type.GetProperty(property.Name, true)

                          select (Expression)Expression.Property(source, realProp);


        return propRequest.GetValueOrDefault(() => base.VisitMember(node));
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        var methodRequest = from source in node.Object.ToMaybe()

                            where source.Type != node.Method.ReflectedType

                            let realMethod = source.Type.GetMethod(node.Method.Name, true)

                            select (Expression)Expression.Call(source, realMethod, node.Arguments);

        return methodRequest.GetValueOrDefault(() => base.VisitMethodCall(node));
    }
}

