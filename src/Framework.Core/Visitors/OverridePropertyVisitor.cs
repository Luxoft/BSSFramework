using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

using Anch.Core;

namespace Framework.Core.Visitors;

public class OverridePropertyVisitor : ExpressionVisitor
{
    private readonly PropertyInfo propertyInfo;
    private readonly LambdaExpression expression;

    public OverridePropertyVisitor(PropertyInfo propertyInfo, LambdaExpression expression)
    {
        if (propertyInfo is null) throw new ArgumentNullException(nameof(propertyInfo));
        if (expression is null) throw new ArgumentNullException(nameof(expression));

        var getter = propertyInfo.GetGetMethod(nonPublic: true);
        if (getter is null) throw new ArgumentException("Property has no getter", nameof(propertyInfo));
        if (expression.Parameters.Count != 1) throw new Exception("Expression must have exactly 1 parameter (the instance)");

        this.propertyInfo = propertyInfo;
        this.expression = expression;
    }

    [return: NotNullIfNotNull("node")]
    public override Expression? Visit(Expression? node)
    {
        return base.Visit(node);
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        if (node.Member == this.propertyInfo && node.Expression is not null)
            return this.GetExpressionByArg(node.Expression);

        return base.VisitMember(node);
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        var getter = this.propertyInfo.GetGetMethod(nonPublic: true);
        if (node.Method == getter && node.Object is not null)
            return this.GetExpressionByArg(node.Object);

        return base.VisitMethodCall(node);
    }

    private Expression GetExpressionByArg(Expression instance)
    {
        var parameter = this.expression.Parameters[0];
        return this.expression.Body.Override(parameter, instance);
    }
}

public class OverridePropertyVisitor<TInstance, TResult>(
    Expression<Func<TInstance, TResult>> propertySelector,
    Expression<Func<TInstance, TResult>> expression)
    : OverridePropertyVisitor(
        (PropertyInfo)((MemberExpression)propertySelector.UpdateBody(FixPropertySourceVisitor.Value).Body).Member,
        expression);
