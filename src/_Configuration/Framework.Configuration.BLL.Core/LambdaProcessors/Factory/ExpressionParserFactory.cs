using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.ExpressionParsers;

namespace Framework.Configuration.BLL
{
    public class ExpressionParserFactory : DynamicExpressionParserFactory, IExpressionParserFactory
    {
        public ExpressionParserFactory(INativeExpressionParser parser)
            : base (parser)
        {

        }


        public SubscriptionConditionLambdaProcessor<TDomainObject> GetBySubscriptionCondition<TDomainObject>()
        {
            return this.GetValue(() => new SubscriptionConditionLambdaProcessor<TDomainObject>(this.Parser));
        }

        public SubscriptionConditionLambdaProcessor<TBLLContext, TDomainObject> GetBySubscriptionCondition<TBLLContext, TDomainObject>()
        {
            return this.GetValue(() => new SubscriptionConditionLambdaProcessor<TBLLContext, TDomainObject>(this.Parser));
        }


        public SubscriptionGenerationLambdaProcessor<TDomainObject> GetBySubscriptionGeneration<TDomainObject>()
        {
            return this.GetValue(() => new SubscriptionGenerationLambdaProcessor<TDomainObject>(this.Parser));
        }

        public SubscriptionGenerationLambdaProcessor<TBLLContext, TDomainObject> GetBySubscriptionGeneration<TBLLContext, TDomainObject>()
        {
            return this.GetValue(() => new SubscriptionGenerationLambdaProcessor<TBLLContext, TDomainObject>(this.Parser));
        }

        /// <inheritdoc/>
        public SubscriptionCopyGenerationLambdaProcessor<TDomainObject> GetBySubscriptionCopyGeneration<TDomainObject>()
        {
            return this.GetValue(() => new SubscriptionCopyGenerationLambdaProcessor<TDomainObject>(this.Parser));
        }

        /// <inheritdoc/>
        public SubscriptionCopyGenerationLambdaProcessor<TBLLContext, TDomainObject> GetBySubscriptionCopyGeneration<TBLLContext, TDomainObject>()
        {
            return this.GetValue(() => new SubscriptionCopyGenerationLambdaProcessor<TBLLContext, TDomainObject>(this.Parser));
        }

        public SubscriptionReplyToGenerationLambdaProcessor<TDomainObject> GetBySubscriptionReplyToGeneration<TDomainObject>()
        {
            return this.GetValue(() => new SubscriptionReplyToGenerationLambdaProcessor<TDomainObject>(this.Parser));
        }

        /// <inheritdoc/>
        public SubscriptionReplyToGenerationLambdaProcessor<TBLLContext, TDomainObject> GetBySubscriptionReplyToGeneration<TBLLContext, TDomainObject>()
        {
            return this.GetValue(() => new SubscriptionReplyToGenerationLambdaProcessor<TBLLContext, TDomainObject>(this.Parser));
        }



        public SubscriptionDynamicSourceLegacyLambdaProcessor<TDomainObject> GetBySubscriptionDynamicSourceLegacy<TDomainObject>()
        {
            return this.GetValue(() => new SubscriptionDynamicSourceLegacyLambdaProcessor<TDomainObject>(this.Parser));
        }

        public SubscriptionDynamicSourceLegacyLambdaProcessor<TBLLContext, TDomainObject> GetBySubscriptionDynamicSourceLegacy<TBLLContext, TDomainObject>()
        {
            return this.GetValue(() => new SubscriptionDynamicSourceLegacyLambdaProcessor<TBLLContext, TDomainObject>(this.Parser));
        }


        public SubscriptionDynamicSourcePrincipalsLambdaProcessor<TDomainObject> GetBySubscriptionDynamicSourcePrincipals<TDomainObject>()
        {
            return this.GetValue(() => new SubscriptionDynamicSourcePrincipalsLambdaProcessor<TDomainObject>(this.Parser));
        }

        public SubscriptionDynamicSourcePrincipalsLambdaProcessor<TBLLContext, TDomainObject> GetBySubscriptionDynamicSourcePrincipals<TBLLContext, TDomainObject>()
        {
            return this.GetValue(() => new SubscriptionDynamicSourcePrincipalsLambdaProcessor<TBLLContext, TDomainObject>(this.Parser));
        }


        public SubscriptionSecurityItemSourceLambdaProcessor<TDomainObject, TSecurityObject> GetBySubscriptionSecurityItemSource<TDomainObject, TSecurityObject>()
        {
            return this.GetValue(() => new SubscriptionSecurityItemSourceLambdaProcessor<TDomainObject, TSecurityObject>(this.Parser));
        }

        public SubscriptionSecurityItemSourceLambdaProcessor<TBLLContext, TDomainObject, TSecurityObject> GetBySubscriptionSecurityItemSource<TBLLContext, TDomainObject, TSecurityObject>()
        {
            return this.GetValue(() => new SubscriptionSecurityItemSourceLambdaProcessor<TBLLContext, TDomainObject, TSecurityObject>(this.Parser));
        }



        public AuthSubscriptionLambdaCompositeExpressionParser<TBLLContext, TDomainObject> GetAuthSubscriptionLambdaExpressionParser<TBLLContext, TDomainObject>(IEnumerable<Type> authTypes)
        {
            if (authTypes == null) throw new ArgumentNullException(nameof(authTypes));

            var authTypeCache = authTypes.ToArray();

            return this.GetValue(() => new AuthSubscriptionLambdaCompositeExpressionParser<TBLLContext, TDomainObject>(this.Parser, authTypeCache), authTypeCache, ArrayComparer<Type>.Value);
        }

        public AuthSubscriptionLambdaCompositeExpressionParser<TDomainObject> GetAuthSubscriptionLambdaExpressionParser<TDomainObject>(IEnumerable<Type> authTypes)
        {
            if (authTypes == null) throw new ArgumentNullException(nameof(authTypes));

            var authTypeCache = authTypes.ToArray();

            return this.GetValue(() => new AuthSubscriptionLambdaCompositeExpressionParser<TDomainObject>(this.Parser, authTypeCache), authTypeCache, ArrayComparer<Type>.Value);
        }


        public SubscriptionLambdaExpressionParser<TDelegate> GetSubscriptionLambdaExpressionParser<TDelegate>()
            where TDelegate : class
        {
            return this.GetValue(() => new SubscriptionLambdaExpressionParser<TDelegate>(this.Parser));
        }

        public RegularJobFunctionExpressionParser<T> GetRegularJobFunctionExpressionParser<T>()
        {
            return this.GetValue(() => new RegularJobFunctionExpressionParser<T>(this.Parser));
        }
    }
}
