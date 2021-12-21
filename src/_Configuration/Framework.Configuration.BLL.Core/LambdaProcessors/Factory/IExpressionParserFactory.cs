using System;
using System.Collections.Generic;

namespace Framework.Configuration.BLL
{
    public interface IExpressionParserFactory
    {
        SubscriptionConditionLambdaProcessor<TDomainObject> GetBySubscriptionCondition<TDomainObject>();

        SubscriptionConditionLambdaProcessor<TBLLContext, TDomainObject> GetBySubscriptionCondition<TBLLContext, TDomainObject>();


        SubscriptionGenerationLambdaProcessor<TDomainObject> GetBySubscriptionGeneration<TDomainObject>();

        SubscriptionGenerationLambdaProcessor<TBLLContext, TDomainObject> GetBySubscriptionGeneration<TBLLContext, TDomainObject>();

        /// <summary>
        /// Создаёт экземпляр класса <see cref="SubscriptionCopyGenerationLambdaProcessor{TDomainObject}"/>.
        /// </summary>
        /// <typeparam name="TDomainObject">Тип доменного объекта.</typeparam>
        /// <returns>Созданный экземпляр процессора ламбда-выражения.</returns>
        SubscriptionCopyGenerationLambdaProcessor<TDomainObject> GetBySubscriptionCopyGeneration<TDomainObject>();

        /// <summary>
        /// Создаёт экземпляр класса <see cref="SubscriptionCopyGenerationLambdaProcessor{TBLLContext, TDomainObject}"/>.
        /// </summary>
        /// <typeparam name="TBLLContext">Тип контекста бизнес-логики.</typeparam>
        /// <typeparam name="TDomainObject">Тип доменного объекта.</typeparam>
        /// <returns>Созданный экземпляр процессора ламбда-выражения.</returns>
        SubscriptionCopyGenerationLambdaProcessor<TBLLContext, TDomainObject> GetBySubscriptionCopyGeneration<TBLLContext, TDomainObject>();

        SubscriptionDynamicSourceLegacyLambdaProcessor<TDomainObject> GetBySubscriptionDynamicSourceLegacy<TDomainObject>();

        SubscriptionDynamicSourceLegacyLambdaProcessor<TBLLContext, TDomainObject> GetBySubscriptionDynamicSourceLegacy<TBLLContext, TDomainObject>();


        SubscriptionDynamicSourcePrincipalsLambdaProcessor<TDomainObject> GetBySubscriptionDynamicSourcePrincipals<TDomainObject>();

        SubscriptionDynamicSourcePrincipalsLambdaProcessor<TBLLContext, TDomainObject> GetBySubscriptionDynamicSourcePrincipals<TBLLContext, TDomainObject>();


        SubscriptionSecurityItemSourceLambdaProcessor<TDomainObject, TSecurityObject> GetBySubscriptionSecurityItemSource<TDomainObject, TSecurityObject>();

        SubscriptionSecurityItemSourceLambdaProcessor<TBLLContext, TDomainObject, TSecurityObject> GetBySubscriptionSecurityItemSource<TBLLContext, TDomainObject, TSecurityObject>();


        AuthSubscriptionLambdaCompositeExpressionParser<TBLLContext, TDomainObject> GetAuthSubscriptionLambdaExpressionParser<TBLLContext, TDomainObject>(IEnumerable<Type> authTypes);

        AuthSubscriptionLambdaCompositeExpressionParser<TDomainObject> GetAuthSubscriptionLambdaExpressionParser<TDomainObject>(IEnumerable<Type> authTypes);



        SubscriptionLambdaExpressionParser<TDelegate> GetSubscriptionLambdaExpressionParser<TDelegate>()
            where TDelegate : class;


        RegularJobFunctionExpressionParser<TBLLContext> GetRegularJobFunctionExpressionParser<TBLLContext>();

        /// <summary>
        /// Создаёт экземпляр класса <see cref="SubscriptionCopyGenerationLambdaProcessor{TDomainObject}"/>.
        /// </summary>
        /// <typeparam name="TDomainObject">Тип доменного объекта.</typeparam>
        /// <returns>Созданный экземпляр процессора ламбда-выражения.</returns>
        SubscriptionReplyToGenerationLambdaProcessor<TDomainObject> GetBySubscriptionReplyToGeneration<TDomainObject>();

        /// <summary>
        /// Создаёт экземпляр класса <see cref="SubscriptionCopyGenerationLambdaProcessor{TBLLContext, TDomainObject}"/>.
        /// </summary>
        /// <typeparam name="TBLLContext">Тип контекста бизнес-логики.</typeparam>
        /// <typeparam name="TDomainObject">Тип доменного объекта.</typeparam>
        /// <returns>Созданный экземпляр процессора ламбда-выражения.</returns>
        SubscriptionReplyToGenerationLambdaProcessor<TBLLContext, TDomainObject> GetBySubscriptionReplyToGeneration<TBLLContext, TDomainObject>();


    }
}
