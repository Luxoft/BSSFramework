using System;

using Framework.ExpressionParsers;

using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL
{
    public class SubscriptionConditionLambdaProcessor<TDomainObject> : SubscriptionLambdaObjectProcessorBase<Subscription, Func<TDomainObject, TDomainObject, bool>>
    {
        public SubscriptionConditionLambdaProcessor(INativeExpressionParser parser)
            : base(parser, subscription => subscription.Condition)
        {

        }
    }

    public class SubscriptionConditionLambdaProcessor<TBLLContext, TDomainObject> : SubscriptionLambdaObjectProcessorBase<Subscription, Func<TBLLContext, TDomainObject, TDomainObject, bool>>
    {
        public SubscriptionConditionLambdaProcessor(INativeExpressionParser parser)
            : base(parser, subscription => subscription.Condition)
        {

        }
    }
}