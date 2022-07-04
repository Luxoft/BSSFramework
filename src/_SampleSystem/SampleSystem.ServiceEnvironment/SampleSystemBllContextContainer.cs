using System;
using System.Collections.Generic;

using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.Authorization.Events;
using Framework.Configuration.BLL;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.SecuritySystem.Rules.Builders;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.Events;
using Framework.Notification.DTO;
using Framework.Notification.New;
using Framework.NotificationCore.Services;
using Framework.NotificationCore.Settings;
using Framework.Persistent;
using Framework.Report;
using Framework.Security.Cryptography;
using Framework.SecuritySystem;
using Framework.Validation;

using SampleSystem.BLL;
using SampleSystem.Domain;
using SampleSystem.Events;

using DomainObjectBase = SampleSystem.Domain.DomainObjectBase;
using PersistentDomainObjectBase = SampleSystem.Domain.PersistentDomainObjectBase;
using Principal = Framework.Authorization.Domain.Principal;

namespace SampleSystem.ServiceEnvironment
{
    public class SampleSystemBllContextContainer : SampleSystemServiceEnvironment.ServiceEnvironmentBLLContextContainer
    {
        private readonly BLLOperationEventListenerContainer<DomainObjectBase> mainOperationListeners = new BLLOperationEventListenerContainer<DomainObjectBase>();

        private readonly BLLSourceEventListenerContainer<PersistentDomainObjectBase> mainSourceListeners = new BLLSourceEventListenerContainer<PersistentDomainObjectBase>();

        private readonly IEventsSubscriptionManager<ISampleSystemBLLContext, PersistentDomainObjectBase> aribaSubscriptionManager;


        private readonly ValidatorCompileCache defaultAuthorizationValidatorCompileCache;

        private readonly ValidatorCompileCache validatorCompileCache;

        private readonly Func<ISampleSystemBLLContext, ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid>> securityExpressionBuilderFactoryFunc;

        private readonly IFetchService<PersistentDomainObjectBase, FetchBuildRule> fetchService;

        private readonly ICryptService<CryptSystem> cryptService;

        private readonly ITypeResolver<string> currentTargetSystemTypeResolver;

        private readonly SmtpSettings smtpSettings;

        private readonly IRewriteReceiversService rewriteReceiversService;

        public SampleSystemBllContextContainer(
            SampleSystemServiceEnvironment serviceEnvironment,
            IServiceProvider scopedServiceProvider,
            ValidatorCompileCache defaultAuthorizationValidatorCompileCache,
            ValidatorCompileCache validatorCompileCache,
            Func<ISampleSystemBLLContext, ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid>> securityExpressionBuilderFactoryFunc,
            IFetchService<PersistentDomainObjectBase, FetchBuildRule> fetchService,
            ICryptService<CryptSystem> cryptService,
            ITypeResolver<string> currentTargetSystemTypeResolver,
            IDBSession session,
            string currentPrincipalName,
            SmtpSettings smtpSettings,
            IRewriteReceiversService rewriteReceiversService)
            : base(serviceEnvironment, scopedServiceProvider, session, currentPrincipalName)
        {
            this.defaultAuthorizationValidatorCompileCache = defaultAuthorizationValidatorCompileCache;
            this.validatorCompileCache = validatorCompileCache;
            this.securityExpressionBuilderFactoryFunc = securityExpressionBuilderFactoryFunc;
            this.fetchService = fetchService;
            this.cryptService = cryptService;
            this.currentTargetSystemTypeResolver = currentTargetSystemTypeResolver;

            this.aribaSubscriptionManager = LazyInterfaceImplementHelper.CreateProxy<IEventsSubscriptionManager<ISampleSystemBLLContext, PersistentDomainObjectBase>>(
                () => new SampleSystemAribaEventsSubscriptionManager(this.MainContext, new SampleSystemAribaLocalDBEventMessageSender(this.MainContext, this.Configuration)));


            this.smtpSettings = smtpSettings;
            this.rewriteReceiversService = rewriteReceiversService;
        }


        protected override ISampleSystemBLLContext CreateMainContext()
        {
            var validator = LazyInterfaceImplementHelper.CreateProxy<IValidator>(() => new SampleSystemValidator(this.MainContext, this.validatorCompileCache));

            var securityExpressionBuilderFactory = this.securityExpressionBuilderFactoryFunc ?? this.GetSecurityExpressionBuilderFactory<ISampleSystemBLLContext, PersistentDomainObjectBase, Guid>;

            return new SampleSystemBLLContext(
                this.ScopedServiceProvider,
                this.Session.GetDALFactory<SampleSystem.Domain.PersistentDomainObjectBase, Guid>(),
                this.mainOperationListeners,
                this.mainSourceListeners,
                this.Session.GetObjectStateService(),
                this.GetAccessDeniedExceptionService<SampleSystem.Domain.PersistentDomainObjectBase, Guid>(),
                this.StandartExpressionBuilder,
                validator,
                this.HierarchicalObjectExpanderFactory,
                this.fetchService,
                this.GetDateTimeService(),
                LazyInterfaceImplementHelper.CreateProxy<ISampleSystemSecurityService>(() => new SampleSystemSecurityService(this.MainContext)),
                LazyInterfaceImplementHelper.CreateProxy(() => securityExpressionBuilderFactory(this.MainContext)),
                LazyInterfaceImplementHelper.CreateProxy<ISampleSystemBLLFactoryContainer>(() => new SampleSystemBLLFactoryContainer(this.MainContext)),
                this.Authorization,
                this.Configuration,
                this.cryptService,
                this.Impersonate,
                this.currentTargetSystemTypeResolver,
                this.Session);
        }

        public override ISecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> GetSecurityExpressionBuilderFactory<TBLLContext, TPersistentDomainObjectBase, TIdent>(TBLLContext context)
        {
            var materialized = new Framework.SecuritySystem.Rules.Builders.MaterializedPermissions.SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent>(context.HierarchicalObjectExpanderFactory, context.Authorization);

            var queryable = new Framework.SecuritySystem.Rules.Builders.QueryablePermissions.SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent>(context.HierarchicalObjectExpanderFactory, context.Authorization);

            return new Framework.SecuritySystem.Rules.Builders.Mixed.SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent>(materialized, queryable);
        }

        /// <summary>
        /// Пример переопределения валидатора для авторизации
        /// </summary>
        /// <returns></returns>
        protected override AuthorizationValidator CreateAuthorizationValidator()
        {
            return new SampleSystemCustomAuthValidator(this.Authorization, this.defaultAuthorizationValidatorCompileCache);
        }
        protected override IMessageSender<Exception> GetExceptionSender()
        {
            return MessageSender<Exception>.Trace;
        }

        ///// <inheritdoc />
        //protected override IMessageSender<Exception> GetExceptionSender()
        //{
        //    return MessageSender<Exception>.Trace;

        //    return new SampleSystemExceptionMessageSender(
        //                                                  this.Configuration,
        //                                                  SmtpMessageSender.Configuration,
        //                                                  this.ServiceEnvironment.NotificationContext.Sender,
        //                                                  this.ServiceEnvironment.NotificationContext.ExceptionReceivers);
        //}

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

        protected override IEnumerable<Framework.Configuration.BLL.ITargetSystemService> GetConfigurationTargetSystemServices(SubscriptionMetadataStore subscriptionMetadataStore)
        {
            yield return this.GetMainConfigurationTargetSystemService();
            yield return this.GetAuthorizationConfigurationTargetSystemService();
            yield return this.GetConfigurationConfigurationTargetSystemService();
        }

        protected override IBLLSimpleQueryBase<IEmployee> GetEmployeeSource(BLLSecurityMode securityMode)
        {
            return this.MainContext.Logics.EmployeeFactory.Create(securityMode);
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
                new Framework.NotificationCore.Senders.SmtpMessageSender(LazyHelper.Create(() => this.smtpSettings), LazyHelper.Create(() => this.rewriteReceiversService), this.Configuration);

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
