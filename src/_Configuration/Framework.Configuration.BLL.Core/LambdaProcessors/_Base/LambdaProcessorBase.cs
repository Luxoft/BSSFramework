using System;
using System.Linq.Expressions;

using Framework.Core;
using Framework.ExpressionParsers;
using Framework.DomainDriven.BLL;
using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL
{
    public abstract class SubscriptionLambdaObjectProcessorBase<TDomainObject, TDelegate> : DomainObjectExpressionParser<TDomainObject, SubscriptionLambda, TDelegate>
        where TDomainObject : PersistentDomainObjectBase
        where TDelegate : class
    {
        protected SubscriptionLambdaObjectProcessorBase(INativeExpressionParser parser, Expression<Func<TDomainObject, SubscriptionLambda>> lambdaPath)
            : base(parser, lambdaPath)
        {

        }


        protected override string GetFormattedDomainObject(TDomainObject domainObject)
        {
            var baseFomatted = base.GetFormattedDomainObject(domainObject);

            return (domainObject as Subscription).Maybe(subscription => $"{baseFomatted} ({subscription.Code})")
                ?? (domainObject as SubscriptionSecurityItem).Maybe(subscriptionSecurityItem =>
                                                                    $"{baseFomatted} (Subscription: {subscriptionSecurityItem.Subscription.Code})")
                ?? baseFomatted;
        }
    }
}