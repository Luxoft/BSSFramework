using System;
using System.Reflection;

using Framework.Configuration.BLL.SubscriptionSystemService3.Lambdas;
using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.Persistent;

using JetBrains.Annotations;

using Serilog;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Services
{
    /// <summary>
    /// Служба тестирования подписок.
    /// </summary>
    public class TestingService<TBLLContext>
        where TBLLContext : class
    {
        private readonly ConfigurationContextFacade configurationContextFacade;
        private readonly SubscriptionServicesFactory<TBLLContext> servicesFactory;
        private readonly ILogger logger;

        /// <summary>Создаёт экземпляр класса <see cref="TestingService"/>.</summary>
        /// <param name="servicesFactory">
        ///     Экземпляр фабрики служб, используемых системой подписок,
        ///     уведомлений и версий доменных объектов.
        /// </param>
        /// <param name="configurationContextFacade">Фасада контекста конфигурации.</param>
        /// <exception cref="ArgumentNullException">Аргумент
        /// servicesFactory
        /// или
        /// configurationContextFacade равен null.
        /// </exception>
        public TestingService(
            [NotNull] SubscriptionServicesFactory<TBLLContext> servicesFactory,
            [NotNull] ConfigurationContextFacade configurationContextFacade)
        {
            this.servicesFactory = servicesFactory ?? throw new ArgumentNullException(nameof(servicesFactory));
            this.configurationContextFacade = configurationContextFacade ?? throw new ArgumentNullException(nameof(configurationContextFacade));

            this.logger = Log.Logger.ForContext(this.GetType());
        }

        /// <summary>Запускает тестовую рассылку уведомлений по подписке.</summary>
        /// <param name="subscription">Подписка.</param>
        /// <param name="domainObjectId">Идентификатор доменного объекта.</param>
        /// <param name="revisionNumber">Номер версии доменного объекта.</param>
        /// <returns>Экземпляр <see cref="ITryResult{Subscription}"/>.</returns>
        /// <exception cref="ArgumentNullException">Аргумент subscription равен null.</exception>
        public virtual ITryResult<Subscription> TestSubscription(
            [NotNull] Subscription subscription,
            Guid domainObjectId,
            long? revisionNumber)
        {
            if (subscription == null)
            {
                throw new ArgumentNullException(nameof(subscription));
            }

            this.logger.Information("Run subscription '{subscription}' in test mode.", subscription);

            var type = this.configurationContextFacade.GetDomainObjectType(subscription.DomainType);

            var method = this.GetType().GetMethod(
                nameof(this.TestSubscriptionTyped),
                BindingFlags.Instance | BindingFlags.NonPublic);

            var generic = method.MakeGenericMethod(type);

            var result = generic.Invoke<ITryResult<Subscription>>(
                this,
                new object[] { subscription, domainObjectId, revisionNumber });

            this.logger.Information("Subscription '{subscription}' test run ended '{result}'.", subscription, result.IsFault() ? $"with error '{((IFaultResult<Subscription>)result).Error}'" : "successfully");

            return result;
        }

        [UsedImplicitly]
        private ITryResult<Subscription> TestSubscriptionTyped<T>(
            Subscription subscription,
            Guid domainObjectId,
            long? revisionNumber)
            where T : class, IIdentityObject<Guid>
        {
            var result = TryResult.Catch(() => this.SendSubscription<T>(subscription, domainObjectId, revisionNumber));
            return result;
        }

        private Subscription SendSubscription<T>(Subscription subscription, Guid domainObjectId, long? revisionNumber)
            where T : class, IIdentityObject<Guid>
        {
            var versions = this.GetObjectVersions<T>(domainObjectId, revisionNumber);

            if (versions == null)
            {
                throw new SubscriptionServicesException($"No active subsciptions for DomainType '{typeof(T)}'.");
            }

            this.SendSubscription(subscription, versions);
            return subscription;
        }

        private void SendSubscription<T>(Subscription subscription, DomainObjectVersions<T> versions)
            where T : class, IIdentityObject<Guid>
        {
            if (!this.IsValidSubscription(subscription, versions))
            {
                var message = $"Subscription condition lambda '{subscription.Condition.Value}' returns false;";
                throw new SubscriptionServicesException(message);
            }

            var notificationService = this.servicesFactory.CreateNotificationService();
            notificationService.NotifyDomainObjectChanged(subscription, versions);
        }

        private bool IsValidSubscription<T>(Subscription subscription, DomainObjectVersions<T> versions)
            where T : class, IIdentityObject<Guid>
        {
            var conditionProcessor = this.servicesFactory.CreateLambdaProcessor<ConditionLambdaProcessor<TBLLContext>>();
            var result = conditionProcessor.Invoke(subscription, versions);

            return result;
        }

        private DomainObjectVersions<T> GetObjectVersions<T>(Guid objectId, long? revision)
            where T : class, IIdentityObject<Guid>
        {
            var revisionService = this.CreateRevisionService<T>();
            var versions = revisionService.GetDomainObjectVersions(objectId, revision);
            return versions;
        }

        private RevisionService<T> CreateRevisionService<T>()
            where T : class, IIdentityObject<Guid>
        {
            return (RevisionService<T>)this.servicesFactory.CreateRevisionService(typeof(T));
        }
    }
}
