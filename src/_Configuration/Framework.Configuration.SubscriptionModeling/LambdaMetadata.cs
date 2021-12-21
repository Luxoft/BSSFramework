using System;
using Framework.Configuration.Core;

namespace Framework.Configuration.SubscriptionModeling
{
    /// <summary>
    ///     Класс экземпляра конфигурации лямбда-выражения подписки.
    /// </summary>
    /// <typeparam name="TContext">Текущий контекст бизнес-логики.</typeparam>
    /// <typeparam name="TDomainObject">Тип доменного объекта.</typeparam>
    /// <typeparam name="TResult">Тип возвращаемого выражением значения.</typeparam>
    public abstract class LambdaMetadata<TContext, TDomainObject, TResult> : ILambdaMetadata
        where TDomainObject : class
    {
        /// <summary>
        ///     Получает лямбда-выражение.
        /// </summary>
        /// <value>
        ///     Лямбда-выражение.
        /// </value>
        public virtual Func<TContext, DomainObjectVersions<TDomainObject>, TResult> Lambda { get; protected set; }

        /// <summary>Получает делегат, исполняющий лямбду.</summary>
        /// <value>Делегат, исполняющий лямбду.</value>
        Func<object, object, object> ILambdaMetadata.Lambda
        {
            get
            {
                return
                    (context, versions) =>
                        this.Lambda((TContext)context, (DomainObjectVersions<TDomainObject>)versions);
            }
        }

        public virtual DomainObjectChangeType DomainObjectChangeType { get; protected set; }

        /// <inheritdoc />
        public void Validate()
        {
            if (this.Lambda == null)
            {
                throw new SubscriptionModelingException($"Property Lambda for type {this.GetType().FullName} must be specified.");
            }
        }
    }
}
