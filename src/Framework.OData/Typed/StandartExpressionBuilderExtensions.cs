using System.Linq.Expressions;

using CommonFramework;

using Framework.Core;
using Framework.Core.Visitors;
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

        var projectionSelectOperation = expressionBuilder.ToTyped<TProjection>(selectOperation);

        var baseSelectOperation = projectionSelectOperation.Covariance<TDomainObject>();

        var standartSelectOperation = baseSelectOperation.Visit(new ExpandProjectionVisitor(typeof(TProjection)));

        return standartSelectOperation;
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
    private readonly Type projectionType;


    public ExpandProjectionVisitor(Type projectionType)
    {
        if (projectionType == null) throw new ArgumentNullException(nameof(projectionType));

        this.projectionType = projectionType;
    }


    public override Expression? Visit(Expression? node)
    {
        var accumVisitor = this.projectionType.GetReferencedTypes(property => property.PropertyType.IsInterface)
                               .Select(refType => new OverrideCallInterfacePropertiesVisitor(refType))
                               .Concat(new ExpressionVisitor[] { ExpandPathVisitor.Value, ExpandExplicitPropertyVisitor.Value })
                               .ToCyclic();

        return accumVisitor.Visit(node);
    }
}
