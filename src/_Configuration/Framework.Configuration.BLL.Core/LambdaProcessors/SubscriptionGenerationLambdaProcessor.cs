using System;
using System.Collections.Generic;

using Framework.ExpressionParsers;

using Framework.Notification;
using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL
{
    public class SubscriptionGenerationLambdaProcessor<TDomainObject> : SubscriptionLambdaObjectProcessorBase<Subscription, Func<TDomainObject, TDomainObject, IEnumerable<NotificationMessageGenerationInfo>>>
    {
        public SubscriptionGenerationLambdaProcessor(INativeExpressionParser parser)
            : base(parser, subscription => subscription.Generation)
        {

        }
    }

    public class SubscriptionGenerationLambdaProcessor<TBLLContext, TDomainObject> : SubscriptionLambdaObjectProcessorBase<Subscription, Func<TBLLContext, TDomainObject, TDomainObject, IEnumerable<NotificationMessageGenerationInfo>>>
    {
        public SubscriptionGenerationLambdaProcessor(INativeExpressionParser parser)
            : base(parser, subscription => subscription.Generation)
        {

        }
    }
}