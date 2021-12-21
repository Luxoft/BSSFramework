using System;
using System.Collections.Generic;

using Framework.ExpressionParsers;
using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL
{
    public class SubscriptionSecurityItemSourceLambdaProcessor<TDomainObject, TSecurityObject> : SubscriptionLambdaObjectProcessorBase<SubscriptionSecurityItem, Func<TDomainObject, TDomainObject, IEnumerable<TSecurityObject>>>
    {
        public SubscriptionSecurityItemSourceLambdaProcessor(INativeExpressionParser parser)
            : base(parser, subscriptionSecurityItem => subscriptionSecurityItem.Source)
        {

        }
    }

    public class SubscriptionSecurityItemSourceLambdaProcessor<TBLLContext, TDomainObject, TSecurityObject> : SubscriptionLambdaObjectProcessorBase<SubscriptionSecurityItem, Func<TBLLContext, TDomainObject, TDomainObject, IEnumerable<TSecurityObject>>>
    {
        public SubscriptionSecurityItemSourceLambdaProcessor(INativeExpressionParser parser)
            : base(parser, subscriptionSecurityItem => subscriptionSecurityItem.Source)
        {

        }
    }
}