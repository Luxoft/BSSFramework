using System;
using System.Linq;

using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Persistent;

using JetBrains.Annotations;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Services
{
    /// <summary>
    /// Типизированная фабрика служб, используемых системой подписок, уведомлений и версий доменных объектов.
    /// </summary>
    /// <typeparam name="T">Тип доменного объекта.</typeparam>
    /// <seealso cref="SubscriptionServicesFactory" />
    public class SubscriptionServicesFactory<TBLLContext, T> : SubscriptionServicesFactory<TBLLContext>
        where TBLLContext : class
        where T : class, IIdentityObject<Guid>
    {
        private readonly IDefaultBLLFactory<T, Guid> defaultBllFactory;

        /// <summary>
        /// Создаёт экземпляр класса <see cref="SubscriptionServicesFactory{T}" />.
        /// </summary>
        /// <param name="configurationContext">Контекст конфигурации.</param>
        /// <param name="defaultBllFactory">Фабрика объектов бизнес-логики.</param>
        /// <param name="bllContext">Контекст бизнес-логики.</param>
        /// <param name="subscriptionMetadataStore">Экземпляр хранилища моделей подписок.</param>
        /// <exception cref="ArgumentNullException">Аргумент
        /// configurationContext
        /// или
        /// defaultBllFactory
        /// или
        /// bllContext
        /// или subscriptionMetadataStore
        /// равен null.</exception>
        public SubscriptionServicesFactory(
            IConfigurationBLLContext configurationContext,
            [NotNull] IDefaultBLLFactory<T, Guid> defaultBllFactory,
            TBLLContext bllContext,
            SubscriptionMetadataStore subscriptionMetadataStore)
            : base(configurationContext, bllContext, subscriptionMetadataStore)
        {
            if (defaultBllFactory == null)
            {
                throw new ArgumentNullException(nameof(defaultBllFactory));
            }

            this.defaultBllFactory = defaultBllFactory;
        }

        /// <summary>Создаёт экземпляр службы поиска версий доменного объекта.</summary>
        /// <param name="domainObjectType">Тип доменного объекта.</param>
        /// <returns>Экземпляр службы поиска версий доменного объекта.</returns>
        public override object CreateRevisionService(Type domainObjectType)
        {
            var method = this.GetType()
                .GetMethods()
                .Single(m => m.Name == nameof(this.CreateRevisionService) && m.IsGenericMethod);

            var @delegate = method.MakeGenericMethod(domainObjectType);
            return @delegate.Invoke<object>(this);
        }

        /// <summary>Создаёт экземпляр службы поиска версий доменного объекта.</summary>
        /// <typeparam name="TDomainObject">Тип доменного объекта.</typeparam>
        /// <returns>Экземпляр службы поиска версий доменного объекта.</returns>
        public virtual RevisionService<TDomainObject> CreateRevisionService<TDomainObject>()
            where TDomainObject : class, T
        {
            var result = new RevisionService<TDomainObject>(
                this.defaultBllFactory.Create<TDomainObject>(),
                this.CreateConfigurationContextFacade(),
                this.CreateSubscriptionsResolver());
            return result;
        }
    }
}
