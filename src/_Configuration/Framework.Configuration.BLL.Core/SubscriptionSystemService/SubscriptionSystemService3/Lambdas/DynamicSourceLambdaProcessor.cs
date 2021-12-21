using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.DomainDriven.BLL.Security;

using JetBrains.Annotations;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Lambdas
{
    /// <summary>
    /// Процессор лямбда-выражения типа "DynamicSource".
    /// </summary>
    /// <seealso cref="LambdaProcessor" />
    public class DynamicSourceLambdaProcessor<TBLLContext> : LambdaProcessor<TBLLContext>
        where TBLLContext : class
    {
        /// <summary>Создаёт экземпляр класса <see cref="DynamicSourceLambdaProcessor"/>.</summary>
        /// <param name="bllContext">Контекст бизнес-логики.</param>
        /// <param name="parserFactory">Фабрика парсеров лямбда-выражений.</param>
        public DynamicSourceLambdaProcessor(
            TBLLContext bllContext,
            IExpressionParserFactory parserFactory)
            : base(bllContext, parserFactory)
        {
        }

        /// <inheritdoc/>
        protected override string LambdaName => "DynamicSource";

        /// <summary>Исполняет указанное в подписке ламбда-выражение типа "DynamicSource".</summary>
        /// <typeparam name="T">Тип доменного объекта.</typeparam>
        /// <param name="subscription">Подписка.</param>
        /// <param name="versions">Версии доменного объекта.</param>
        /// <returns>Результат исполнения лямбда-выражения.</returns>
        /// <exception cref="ArgumentNullException">Аргумент
        /// subscription
        /// или
        /// versions равен null.
        /// </exception>
        public virtual IEnumerable<FilterItemIdentity> Invoke<T>(
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

            var lambda = subscription.DynamicSource;

            if (!DomainObjectCompliesLambdaRequiredMode(lambda, versions))
            {
                return Enumerable.Empty<FilterItemIdentity>();
            }

            var result = this.TryInvoke(subscription, versions, this.InvokeInternal);

            return result;
        }

        private IEnumerable<FilterItemIdentity> InvokeInternal<T>(
            Subscription subscription,
            DomainObjectVersions<T> versions)
            where T : class
        {
            var result = subscription.DynamicSource.WithContext
                ? this.InvokeWithTypedContext(subscription, versions)
                : this.InvokeWithoutContext(subscription, versions);

            return result;
        }

        private IEnumerable<FilterItemIdentity> InvokeWithoutContext<T>(
            Subscription subscription,
            DomainObjectVersions<T> versions)
            where T : class
        {
            var @delegate = this.ParserFactory
                .GetBySubscriptionDynamicSourceLegacy<T>()
                .GetDelegate(subscription);

            var result = @delegate(versions.Previous, versions.Current);

            return result;
        }


        [UsedImplicitly]
        private IEnumerable<FilterItemIdentity> InvokeWithTypedContext<T>(
            Subscription subscription,
            DomainObjectVersions<T> versions)
            where T : class
        {
            var @delegate = this.ParserFactory
                .GetBySubscriptionDynamicSourceLegacy<TBLLContext, T>().GetDelegate(subscription);

            var result = @delegate(this.BllContext, versions.Previous, versions.Current);

            return result;
        }
    }
}
