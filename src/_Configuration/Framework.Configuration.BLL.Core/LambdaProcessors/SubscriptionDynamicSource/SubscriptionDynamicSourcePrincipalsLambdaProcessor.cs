using System;
using System.Collections.Generic;

using Framework.ExpressionParsers;
using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL
{
    public class SubscriptionDynamicSourcePrincipalsLambdaProcessor<TDomainObject> : SubscriptionLambdaObjectProcessorBase<Subscription, Func<TDomainObject, TDomainObject, IEnumerable<string>>>
    {
        public SubscriptionDynamicSourcePrincipalsLambdaProcessor(INativeExpressionParser parser)
            : base(parser, subscription => subscription.DynamicSource)
        {

        }
    }

    public class SubscriptionDynamicSourcePrincipalsLambdaProcessor<TBLLContext, TDomainObject> : SubscriptionLambdaObjectProcessorBase<Subscription, Func<TBLLContext, TDomainObject, TDomainObject, IEnumerable<string>>>
    {
        public SubscriptionDynamicSourcePrincipalsLambdaProcessor(INativeExpressionParser parser)
            : base(parser, subscription => subscription.DynamicSource)
        {

        }
    }
}