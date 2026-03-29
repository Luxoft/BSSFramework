using CommonFramework;

using Framework.OData.QueryLanguage.StandardExpressionBuilder;

namespace Framework.OData.Typed;

public static class StandardExpressionBuilderExtensions
{
    public static SelectOperation<TDomainObject> ToTyped<TDomainObject>(this IStandardExpressionBuilder expressionBuilder, SelectOperation selectOperation)
    {
        if (expressionBuilder == null) throw new ArgumentNullException(nameof(expressionBuilder));
        if (selectOperation == null) throw new ArgumentNullException(nameof(selectOperation));

        var filter = expressionBuilder.ToStandardExpression<Func<TDomainObject, bool>>(selectOperation.Filter);

        var orders = selectOperation.Orders.Select(expressionBuilder.ToTypedOrder<TDomainObject>);

        return new SelectOperation<TDomainObject>(filter, [.. orders], selectOperation.SkipCount, selectOperation.TakeCount)
               {
                   Expands = selectOperation.Expands, Selects = selectOperation.Selects
               };
    }

    private static SelectOrder<TDomainObject> ToTypedOrder<TDomainObject>(this IStandardExpressionBuilder standardExpressionBuilder, SelectOrder selectOrder)
    {
        if (standardExpressionBuilder == null) throw new ArgumentNullException(nameof(standardExpressionBuilder));
        if (selectOrder == null) throw new ArgumentNullException(nameof(selectOrder));

        var orderKeyType = selectOrder.Path.ExtractTargetType<TDomainObject>();

        return new Func<SelectOrder, SelectOrder<TDomainObject, object>>(standardExpressionBuilder.ToTypedOrder<TDomainObject, object>)
               .CreateGenericMethod(typeof(TDomainObject), orderKeyType)
               .Invoke<SelectOrder<TDomainObject>>(null, standardExpressionBuilder, selectOrder);
    }

    private static SelectOrder<TDomainObject, TOrderKey> ToTypedOrder<TDomainObject, TOrderKey>(this IStandardExpressionBuilder standardExpressionBuilder, SelectOrder selectOrder) =>
        new(
        standardExpressionBuilder.ToStandardExpression<Func<TDomainObject, TOrderKey>>(selectOrder.Path)) { OrderType = selectOrder.OrderType };
}
