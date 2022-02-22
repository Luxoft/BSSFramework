using System;

using Framework.Configuration.Core;
using Framework.Configuration.Domain;

using JetBrains.Annotations;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Lambdas
{
    /// <summary>
    /// Процессор лямбда-выражения типа "Condition".
    /// </summary>
    /// <seealso cref="LambdaProcessor" />
    public class ConditionLambdaProcessor<TBLLContext> : LambdaProcessor<TBLLContext>
        where TBLLContext : class
    {
        /// <summary>Создаёт экземпляр класса <see cref="ConditionLambdaProcessor"/>.</summary>
        /// <param name="bllContext">Контекст бизнес-логики.</param>
        /// <param name="parserFactory">Фабрика парсеров лямбда-выражений.</param>
        public ConditionLambdaProcessor(TBLLContext bllContext)
            : base(bllContext)
        {
        }

        /// <inheritdoc/>
        protected override string LambdaName => "Condition";

        /// <summary>Исполняет указанное в подписке ламбда-выражение типа "Condition".</summary>
        /// <typeparam name="T">Тип доменного объекта.</typeparam>
        /// <param name="subscription">Подписка.</param>
        /// <param name="versions">Версии доменного объекта.</param>
        /// <returns>Результат исполнения лямбда-выражения.</returns>
        /// <exception cref="ArgumentNullException">Аргумент
        /// subscription
        /// или
        /// versions равен null.
        /// </exception>
        public virtual bool Invoke<T>([NotNull] Subscription subscription, [NotNull] DomainObjectVersions<T> versions)
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

            var lambda = subscription.Condition;

            if (lambda == null)
            {
                return false;
            }

            var result = DomainObjectCompliesLambdaRequiredMode(lambda, versions) &&
                         this.TryInvoke(subscription, versions, this.InvokeInternal);

            return result;
        }

        private bool InvokeInternal<T>(Subscription subscription, DomainObjectVersions<T> versions)
            where T : class
        {
            var result = this.InvokeWithTypedContext(subscription, versions);

            return result;
        }


        [UsedImplicitly]
        private bool InvokeWithTypedContext<T>(
            Subscription subscription,
            DomainObjectVersions<T> versions)
            where T : class
        {
            bool result;
            var funcValue = subscription.Condition.FuncValue;

            result = this.TryCast<bool>(funcValue(this.BllContext, versions));

            return result;
        }
    }
}
