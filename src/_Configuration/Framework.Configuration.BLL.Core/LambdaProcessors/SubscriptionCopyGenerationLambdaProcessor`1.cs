using System;
using System.Collections.Generic;
using Framework.Configuration.Domain;
using Framework.ExpressionParsers;

using Framework.Notification;

namespace Framework.Configuration.BLL
{
    /// <summary>
    /// Процессор лямбда-выражения типа "CopyGeneration".
    /// </summary>
    /// <typeparam name="TDomainObject">Тип доменного объекта.</typeparam>
    /// <seealso cref="SubscriptionLambdaObjectProcessorBase{Subscription, Func}" />
    public class SubscriptionCopyGenerationLambdaProcessor<TDomainObject>
        : SubscriptionLambdaObjectProcessorBase<Subscription, Func<TDomainObject, TDomainObject, IEnumerable<NotificationMessageGenerationInfo>>>
    {
        /// <summary>
        /// Создаёт экземпляр класса <see cref="SubscriptionCopyGenerationLambdaProcessor{TDomainObject}"/>.
        /// </summary>
        /// <param name="parser">Парсер лямбда-выражения.</param>
        public SubscriptionCopyGenerationLambdaProcessor(INativeExpressionParser parser)
            : base(parser, subscription => subscription.CopyGeneration)
        {
        }
    }
}
