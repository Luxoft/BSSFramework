using System;
using System.Collections.Generic;

using Framework.ExpressionParsers;
using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL
{
    public class SubscriptionDynamicSourceLegacyLambdaProcessor<TDomainObject> : SubscriptionLambdaObjectProcessorBase<Subscription, Func<TDomainObject, TDomainObject, IEnumerable<Framework.DomainDriven.BLL.Security.FilterItemIdentity>>>
    {
        public SubscriptionDynamicSourceLegacyLambdaProcessor(INativeExpressionParser parser)
            : base(parser, subscription => subscription.DynamicSource)
        {

        }
    }

    public class SubscriptionDynamicSourceLegacyLambdaProcessor<TBLLContext, TDomainObject> : SubscriptionLambdaObjectProcessorBase<Subscription, Func<TBLLContext, TDomainObject, TDomainObject, IEnumerable<Framework.DomainDriven.BLL.Security.FilterItemIdentity>>>
    {
        public SubscriptionDynamicSourceLegacyLambdaProcessor(INativeExpressionParser parser)
            : base(parser, subscription => subscription.DynamicSource)
        {

        }
    }
}