using System;
using System.Collections.Generic;

using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.Authorization.Events;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.Events;
using Framework.Notification.DTO;

using SampleSystem.BLL;
using SampleSystem.Events;

using PersistentDomainObjectBase = SampleSystem.Domain.PersistentDomainObjectBase;
using Principal = Framework.Authorization.Domain.Principal;

namespace SampleSystem.ServiceEnvironment
{
    public class SampleSystemBLLContextContainer : SampleSystemServiceEnvironment.ServiceEnvironmentBLLContextContainer
    {
        private readonly IEventsSubscriptionManager<ISampleSystemBLLContext, PersistentDomainObjectBase> aribaSubscriptionManager;

        private readonly SampleSystemServiceEnvironment serviceEnvironment;

        public SampleSystemBLLContextContainer(
            SampleSystemServiceEnvironment serviceEnvironment,
            IServiceProvider scopedServiceProvider,
            IDBSession dbSession)
            : base(serviceEnvironment, scopedServiceProvider, dbSession)
        {
            this.serviceEnvironment = serviceEnvironment;

            this.aribaSubscriptionManager = LazyInterfaceImplementHelper.CreateProxy<IEventsSubscriptionManager<ISampleSystemBLLContext, PersistentDomainObjectBase>>(
                () => new SampleSystemAribaEventsSubscriptionManager(this.MainContext, new SampleSystemAribaLocalDBEventMessageSender(this.MainContext, this.Configuration)));
        }

        /// <inheritdoc />
        protected override IEnumerable<IManualEventDALListener<Framework.Authorization.Domain.PersistentDomainObjectBase>> GetAuthorizationEventDALListeners()
        {
            foreach (var baseListener in base.GetAuthorizationEventDALListeners())
            {
                yield return baseListener;
            }

            yield return this.GetAuthEventDALListener();
        }

        private IManualEventDALListener<Framework.Authorization.Domain.PersistentDomainObjectBase> GetAuthEventDALListener()
        {
            var authEventTypes = new[]
                                 {
                                         TypeEvent.Save<Principal>(),
                                         TypeEvent.SaveAndRemove<Permission>(),
                                         TypeEvent.SaveAndRemove<BusinessRole>()
                                     };

            var dependencyEvents = new[]
                                   {
                                           TypeEventDependency.FromSaveAndRemove<PermissionFilterItem, Permission>(z => z.Permission),
                                           TypeEventDependency.FromSaveAndRemove<Permission, Principal>(z => z.Principal)
                                       };

            var messageSender = new AuthorizationLocalDBEventMessageSender(this.Authorization, this.Configuration, "authDALQuery");

            return new DefaultAuthDALListener(this.Authorization, authEventTypes, messageSender, dependencyEvents);
        }

        protected override IEventsSubscriptionManager<ISampleSystemBLLContext, PersistentDomainObjectBase> CreateMainEventsSubscriptionManager()
        {
            return new SampleSystemEventsSubscriptionManager(
                                                             this.MainContext,
                                                             new SampleSystemLocalDBEventMessageSender(this.MainContext, this.Configuration)); // Sender для отправки евентов в локальную бд
        }

        protected override IEventsSubscriptionManager<IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase> CreateAuthorizationEventsSubscriptionManager()
        {
            return new AuthorizationEventsSubscriptionManager(
                                                              this.Authorization,
                                                              new AuthorizationLocalDBEventMessageSender(this.Authorization, this.Configuration)); // Sender для отправки евентов в локальную бд
        }

        /// <summary>
        /// Сохранение модификаций в локальную бд
        /// </summary>
        /// <returns></returns>
        protected override IStandardSubscriptionService CreateSubscriptionService()
        {
            return new LocalDBSubscriptionService(this.Configuration);
        }

        ///// <summary>
        ///// Сохранение нотификаций в локальной бд, откуда их будет забирать Biztalk
        ///// </summary>
        ///// <returns></returns>
        //protected override IMessageSender<NotificationEventDTO> GetMessageTemplateSender()
        //{
        //    return new LocalDBNotificationEventDTOMessageSender(this.Configuration);
        //}

        protected override IMessageSender<NotificationEventDTO> GetMessageTemplateSender() =>
                new Framework.NotificationCore.Senders.SmtpMessageSender(LazyHelper.Create(() => this.serviceEnvironment.SmtpSettings), LazyHelper.Create(() => this.serviceEnvironment.RewriteReceiversService), this.Configuration);

        /// <summary>
        /// Добавление подписок на евенты для арибы
        /// </summary>
        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();

            this.aribaSubscriptionManager.Subscribe();
        }
    }
}
