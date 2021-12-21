using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.Notification;

using JetBrains.Annotations;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Lambdas
{
    /// <summary>
    /// Базовый класс процессоров лямбда-выражений типа "Generation".
    /// </summary>
    /// <seealso cref="LambdaProcessor" />
    public abstract class GenerationLambdaProcessorBase<TBLLContext> : LambdaProcessor<TBLLContext>
        where TBLLContext : class
    {
        /// <summary>Создаёт экземпляр класса <see cref="GenerationLambdaProcessorBase"/>.</summary>
        /// <param name="bllContext">Контекст бизнес-логики.</param>
        /// <param name="parserFactory">Фабрика парсеров лямбда-выражений.</param>
        protected GenerationLambdaProcessorBase(TBLLContext bllContext, IExpressionParserFactory parserFactory)
            : base(bllContext, parserFactory)
        {
        }

        /// <summary>Исполняет указанное в подписке ламбда-выражение типа "Generation".</summary>
        /// <typeparam name="T">Тип доменного объекта.</typeparam>
        /// <param name="subscription">Подписка.</param>
        /// <param name="versions">Версии доменного объекта.</param>
        /// <returns>Результат исполнения лямбда-выражения.</returns>
        /// <exception cref="ArgumentNullException">Аргумент
        /// subscription
        /// или
        /// versions равен null.
        /// </exception>
        public virtual IEnumerable<NotificationMessageGenerationInfo> Invoke<T>(
            [NotNull] Subscription subscription,
            [NotNull] DomainObjectVersions<T> versions)
            where T : class
        {
            if (subscription == null)
            {
                throw new ArgumentNullException(nameof(subscription));
            }

            if (versions == null)
            {
                throw new ArgumentNullException(nameof(versions));
            }

            var lambda = this.GetSubscriptionLambda(subscription);

            if (!DomainObjectCompliesLambdaRequiredMode(lambda, versions))
            {
                return Enumerable.Empty<NotificationMessageGenerationInfo>();
            }

            var result = this.TryInvoke(subscription, versions, this.InvokeInternal);

            return result;
        }

        /// <summary>Возвращает делегат для вызова лямбда-выражения без контекста бизнес-логики.</summary>
        /// <typeparam name="T">Тип доменного объекта.</typeparam>
        /// <param name="subscription">Подписка.</param>
        /// <returns>Делегат для вызова лямбда-выражения без контекста бизнес-логики.</returns>
        protected abstract Func<T, T, IEnumerable<NotificationMessageGenerationInfo>> GetNonContextDelegate<T>(
            Subscription subscription);

        /// <summary>Возвращает делегат для вызова лямбда-выражения с контекстом бизнес-логики.</summary>
        /// <typeparam name="TBLLContext">Тип контекста бизнес-логики.</typeparam>
        /// <typeparam name="T">Тип доменного объекта.</typeparam>
        /// <param name="subscription">Подписка.</param>
        /// <returns>Делегат для вызова лямбда-выражения с контекстом бизнес-логики.</returns>
        protected abstract Func<TBLLContext, T, T, IEnumerable<NotificationMessageGenerationInfo>> GetContextDelegate
            <T>(Subscription subscription);

        /// <summary>Возвращает лямбда-выражения подписки.</summary>
        /// <param name="subscription">Подписка.</param>
        /// <returns>Лямбда-выражение подписки.</returns>
        protected abstract SubscriptionLambda GetSubscriptionLambda(Subscription subscription);

        /// <summary>Исполняет лямбда-выражение с типизированным контекстом бизнес-логики.</summary>
        /// <typeparam name="T">Тип доменного объекта.</typeparam>
        /// <param name="subscription">Подписка.</param>
        /// <param name="versions">Версии доменного объекта.</param>
        /// <returns>Результат вызова лямбда-выражения.</returns>
        [UsedImplicitly]
        protected IEnumerable<NotificationMessageGenerationInfo> InvokeWithTypedContext<T>(
            Subscription subscription,
            DomainObjectVersions<T> versions)
            where T : class
        {
            IEnumerable<NotificationMessageGenerationInfo> result;
            var funcValue = this.GetSubscriptionLambda(subscription).FuncValue;

            if (funcValue != null)
            {
                result = this.TryCast<IEnumerable<NotificationMessageGenerationInfo>>(funcValue(this.BllContext, versions));
            }
            else
            {
                var @delegate = this.GetContextDelegate<T>(subscription);
                result = @delegate(this.BllContext, versions.Previous, versions.Current);
            }

            return result;
        }

        private IEnumerable<NotificationMessageGenerationInfo> InvokeWithoutContext<T>(
            Subscription subscription,
            DomainObjectVersions<T> versions)
            where T : class
        {
            var @delegate = this.GetNonContextDelegate<T>(subscription);
            var result = @delegate(versions.Previous, versions.Current);

            return result;
        }

        private IEnumerable<NotificationMessageGenerationInfo> InvokeInternal<T>(
            Subscription subscription,
            DomainObjectVersions<T> versions)
            where T : class
        {
            var result = this.GetSubscriptionLambda(subscription).WithContext
                ? this.InvokeWithTypedContext(subscription, versions)
                : this.InvokeWithoutContext(subscription, versions);

            return result;
        }
    }
}
