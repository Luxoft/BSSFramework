using System.Linq.Expressions;

using Framework.Core;
using Framework.Persistent;
using Framework.QueryLanguage;

using Expression = System.Linq.Expressions.Expression;

namespace Framework.OData;

public static class StandartExpressionBuilderExtensions
{
    public static SelectOperation<TDomainObject> ToTyped<TDomainObject>(this IStandartExpressionBuilder expressionBuilder, SelectOperation selectOperation)
    {
        if (expressionBuilder == null) throw new ArgumentNullException(nameof(expressionBuilder));
        if (selectOperation == null) throw new ArgumentNullException(nameof(selectOperation));

        var filter = expressionBuilder.ToStandartExpression<Func<TDomainObject, bool>>(selectOperation.Filter);

        var orders = selectOperation.Orders.ToArray(order => expressionBuilder.ToTypedOrder<TDomainObject>(order));

        return new SelectOperation<TDomainObject>(filter, orders, selectOperation.Expands, selectOperation.Selects, selectOperation.SkipCount, selectOperation.TakeCount);
    }

    public static SelectOperation<TDomainObject> ToTyped<TDomainObject, TProjection>(this IStandartExpressionBuilder expressionBuilder, SelectOperation selectOperation)
            where TDomainObject : TProjection
    {
        if (expressionBuilder == null) throw new ArgumentNullException(nameof(expressionBuilder));
        if (selectOperation == null) throw new ArgumentNullException(nameof(selectOperation));

        var projectionSelectOperaton = expressionBuilder.ToTyped<TProjection>(selectOperation);

        var baseSelectOperaton = projectionSelectOperaton.Covariance<TDomainObject>();

        var standartSelectOperaton = baseSelectOperaton.Visit(new ExpandProjectionVisitor(typeof(TProjection)));

        return standartSelectOperaton;
    }


    private static ISelectOrder<TDomainObject> ToTypedOrder<TDomainObject>(this IStandartExpressionBuilder standartExpressionBuilder, SelectOrder selectOrder)
    {
        if (standartExpressionBuilder == null) throw new ArgumentNullException(nameof(standartExpressionBuilder));
        if (selectOrder == null) throw new ArgumentNullException(nameof(selectOrder));

        var orderKeyType = selectOrder.Path.ExtractTargetType<TDomainObject>();

        return new Func<SelectOrder, SelectOrder<TDomainObject, object>>(standartExpressionBuilder.ToTypedOrder<TDomainObject, object>)
               .CreateGenericMethod(typeof(TDomainObject), orderKeyType)
               .Invoke<ISelectOrder<TDomainObject>>(null, standartExpressionBuilder, selectOrder);
    }

    private static SelectOrder<TDomainObject, TOrderKey> ToTypedOrder<TDomainObject, TOrderKey>(this IStandartExpressionBuilder standartExpressionBuilder, SelectOrder selectOrder)
    {
        return new SelectOrder<TDomainObject, TOrderKey>(
                                                         standartExpressionBuilder.ToStandartExpression<Func<TDomainObject, TOrderKey>>(selectOrder.Path),
                                                         selectOrder.OrderType);
    }
}

public sealed class ExpandProjectionVisitor : ExpressionVisitor
{
    private readonly Type _projectionType;


    public ExpandProjectionVisitor(Type projectionType)
    {
        if (projectionType == null) throw new ArgumentNullException(nameof(projectionType));

        this._projectionType = projectionType;
    }


    public override Expression Visit(Expression node)
    {
        var accumVisitor = this._projectionType.GetReferencedTypes(property => property.PropertyType.IsInterface)
                               .Select(refType => new OverrideCallInterfacePropertiesVisitor(refType))
                               .Concat(new ExpressionVisitor[] { ExpandPathVisitor.Value, ExpandExplicitPropertyVisitor.Value, OverrideCallInterfaceGenericMethodVisitor.Value })
                               .ToCyclic();

        return accumVisitor.Visit(node);
    }


    private class OverrideCallInterfaceGenericMethodVisitor : ExpressionVisitor
    {
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.IsGenericMethodImplementation(MethodInfoHelper.ExpandContainsMethod))
            {
                var argumentTypes = node.Arguments.Take(2).ToArray(arg => arg.Type);

                var methodGenericTypes = node.Method.GetGenericArguments();

                if (methodGenericTypes[0].IsInterface && methodGenericTypes[0] != argumentTypes[0])
                {
                    var newMethod = MethodInfoHelper.ExpandContainsMethod.MakeGenericMethod(argumentTypes);

                    return Expression.Call(newMethod, node.Arguments.Select(this.Visit));
                }
            }

            return base.VisitMethodCall(node);
        }

        public static readonly OverrideCallInterfaceGenericMethodVisitor Value = new OverrideCallInterfaceGenericMethodVisitor();
    }
}
