using System;
using System.Collections.Generic;
using Framework.Configuration.Domain;
using Framework.Notification;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Lambdas
{
    /// <summary>
    /// Процессор лямбда-выражения типа "Generation".
    /// </summary>
    /// <seealso cref="LambdaProcessor" />
    public class GenerationLambdaProcessorTo<TBLLContext> : GenerationLambdaProcessorBase<TBLLContext>
        where TBLLContext : class
    {
        /// <summary>Создаёт экземпляр класса <see cref="GenerationLambdaProcessorTo"/>.</summary>
        /// <param name="bllContext">Контекст бизнес-логики.</param>
        /// <param name="parserFactory">Фабрика парсеров лямбда-выражений.</param>
        public GenerationLambdaProcessorTo(TBLLContext bllContext, IExpressionParserFactory parserFactory)
            : base(bllContext, parserFactory)
        {
        }

        /// <inheritdoc/>
        protected override string LambdaName => "Generation";

        /// <inheritdoc/>
        protected override SubscriptionLambda GetSubscriptionLambda(Subscription subscription)
        {
            return subscription.Generation;
        }

        /// <inheritdoc/>
        protected override Func<T, T, IEnumerable<NotificationMessageGenerationInfo>> GetNonContextDelegate<T>(
            Subscription subscription)
        {
            return this.ParserFactory.GetBySubscriptionGeneration<T>().GetDelegate(subscription);
        }

        /// <inheritdoc/>
        protected override Func<TBLLContext, T, T, IEnumerable<NotificationMessageGenerationInfo>> GetContextDelegate
            <T>(Subscription subscription)
        {
            return this.ParserFactory.GetBySubscriptionGeneration<TBLLContext, T>().GetDelegate(subscription);
        }
    }
}
