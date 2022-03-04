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
using Framework.DomainDriven.BLL.Security;
using Framework.SecuritySystem.Rules.Builders;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.Events;
using Framework.Notification.DTO;
using Framework.Persistent;
using Framework.Security.Cryptography;
using Framework.SecuritySystem;
using Framework.Validation;
using Framework.Workflow.BLL;

using WorkflowSampleSystem.BLL;

using DomainObjectBase = WorkflowSampleSystem.Domain.DomainObjectBase;
using PersistentDomainObjectBase = WorkflowSampleSystem.Domain.PersistentDomainObjectBase;
using Principal = Framework.Authorization.Domain.Principal;

namespace WorkflowSampleSystem.ServiceEnvironment
{
    public class WorkflowSampleSystemBLLContextContainer : WorkflowSampleSystemServiceEnvironment.ServiceEnvironmentBLLContextContainer, IWorkflowBLLContextContainer, IBLLContextContainer<IWorkflowBLLContext>
    {
        private readonly BLLOperationEventListenerContainer<DomainObjectBase> mainOperationListeners = new BLLOperationEventListenerContainer<DomainObjectBase>();

        private readonly BLLSourceEventListenerContainer<PersistentDomainObjectBase> mainSourceListeners = new BLLSourceEventListenerContainer<PersistentDomainObjectBase>();

        private readonly ValidatorCompileCache validatorCompileCache;

        private readonly Func<IWorkflowSampleSystemBLLContext, ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid>> securityExpressionBuilderFactoryFunc;

        private readonly IFetchService<PersistentDomainObjectBase, FetchBuildRule> fetchService;

        private readonly ICryptService<CryptSystem> cryptService;

        private readonly ITypeResolver<string> currentTargetSystemTypeResolver;

        private readonly Lazy<WorkflowSamplSystemBLLContextContainerModule> lazyWorkflowModule;

        public readonly IWorkflowSampleSystemAuthorizationBLLContext WorkflowSampleSystemAuthorization;

        public WorkflowSampleSystemBLLContextContainer(
            WorkflowSampleSystemServiceEnvironment serviceEnvironment,
            IServiceProvider scopedServiceProvider,
            ValidatorCompileCache validatorCompileCache,
            Func<IWorkflowSampleSystemBLLContext, ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid>> securityExpressionBuilderFactoryFunc,
            IFetchService<PersistentDomainObjectBase, FetchBuildRule> fetchService,
            ICryptService<CryptSystem> cryptService,
            ITypeResolver<string> currentTargetSystemTypeResolver,
            IDBSession session,
            string currentPrincipalName)
            : base(serviceEnvironment, scopedServiceProvider, session, currentPrincipalName)
        {
            this.validatorCompileCache = validatorCompileCache;
            this.securityExpressionBuilderFactoryFunc = securityExpressionBuilderFactoryFunc;
            this.fetchService = fetchService;
            this.cryptService = cryptService;
            this.currentTargetSystemTypeResolver = currentTargetSystemTypeResolver;

            this.lazyWorkflowModule = LazyHelper.Create(() => new WorkflowSamplSystemBLLContextContainerModule(serviceEnvironment, this));

            this.WorkflowSampleSystemAuthorization = LazyInterfaceImplementHelper.CreateProxy(this.CreateWorkflowSampleSystemAuthorizationBLLContext);
        }

        public IWorkflowBLLContext Workflow => this.lazyWorkflowModule.Value.Workflow;

        protected override IEnumerable<IBLLContextContainerModule> GetModules()
        {
            foreach (var baseModule in base.GetModules())
            {
                yield return baseModule;
            }

            yield return this.lazyWorkflowModule.Value;
        }

        protected override IWorkflowSampleSystemBLLContext CreateMainContext()
        {
            var validator = LazyInterfaceImplementHelper.CreateProxy<IValidator>(() => new WorkflowSampleSystemValidator(this.MainContext, this.validatorCompileCache));

            var securityExpressionBuilderFactory = this.securityExpressionBuilderFactoryFunc ?? this.GetSecurityExpressionBuilderFactory<IWorkflowSampleSystemBLLContext, PersistentDomainObjectBase, Guid>;

            return new WorkflowSampleSystemBLLContext(
                this.ScopedServiceProvider,
                this.Session.GetDALFactory<WorkflowSampleSystem.Domain.PersistentDomainObjectBase, Guid>(),
                this.mainOperationListeners,
                this.mainSourceListeners,
                this.Session.GetObjectStateService(),
                this.GetAccessDeniedExceptionService<WorkflowSampleSystem.Domain.PersistentDomainObjectBase, Guid>(),
                this.StandartExpressionBuilder,
                validator,
                this.HierarchicalObjectExpanderFactory,
                this.fetchService,
                this.GetDateTimeService(),
                LazyInterfaceImplementHelper.CreateProxy<IWorkflowSampleSystemSecurityService>(() => new WorkflowSampleSystemSecurityService(this.MainContext)),
                LazyInterfaceImplementHelper.CreateProxy(() => securityExpressionBuilderFactory(this.MainContext)),
                LazyInterfaceImplementHelper.CreateProxy<IWorkflowSampleSystemBLLFactoryContainer>(() => new WorkflowSampleSystemBLLFactoryContainer(this.MainContext)),
                this.Authorization,
                this.Configuration,
                this.Workflow,
                this.cryptService,
                this.Impersonate,
                this.currentTargetSystemTypeResolver);
        }

        protected override IAuthorizationBLLContext CreateAuthorizationBLLContext()
        {
            return this.WorkflowSampleSystemAuthorization;
        }

        private IWorkflowSampleSystemAuthorizationBLLContext CreateWorkflowSampleSystemAuthorizationBLLContext()
        {
            return new WorkflowSampleSystemAuthorizationBLLContext(
                this.ScopedServiceProvider,
                this.Session.GetDALFactory<Framework.Authorization.Domain.PersistentDomainObjectBase, Guid>(),
                this.AuthorizationOperationListeners,
                this.AuthorizationSourceListeners,
                this.Session.GetObjectStateService(),
                this.GetAccessDeniedExceptionService<Framework.Authorization.Domain.PersistentDomainObjectBase, Guid>(),
                this.StandartExpressionBuilder,
                LazyInterfaceImplementHelper.CreateProxy<IValidator>(this.CreateAuthorizationValidator),
                this.HierarchicalObjectExpanderFactory,
                this.ServiceEnvironment.AuthorizationFetchService,
                this.GetDateTimeService(),
                this.GetUserAuthenticationService(),
                LazyInterfaceImplementHelper.CreateProxy(() => this.GetSecurityExpressionBuilderFactory<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase, Guid>(this.Authorization)),
                this.Configuration,
                LazyInterfaceImplementHelper.CreateProxy<IAuthorizationSecurityService>(() => new AuthorizationSecurityService(this.Authorization)),
                LazyInterfaceImplementHelper.CreateProxy<IAuthorizationBLLFactoryContainer>(() => new AuthorizationBLLFactoryContainer(this.Authorization)),
                LazyInterfaceImplementHelper.CreateProxy(this.GetAuthorizationExternalSource),
                LazyInterfaceImplementHelper.CreateProxy<IRunAsManager>(() => new AuthorizationRunAsManger(this.Authorization)),
                principalName => this.Impersonate(principalName).Authorization,
                LazyInterfaceImplementHelper.CreateProxy(this.GetSecurityTypeResolver),
                this.GetWorkflowApproveProcessor());
        }

        private IWorkflowApproveProcessor GetWorkflowApproveProcessor()
        {
            return new WorkflowApproveProcessor(this.Authorization);
        }

        protected override IMessageSender<Exception> GetExceptionSender()
        {
            return MessageSender<Exception>.Trace;
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

        protected override IMessageSender<NotificationEventDTO> GetMessageTemplateSender() => new LocalDBNotificationEventDTOMessageSender(this.Configuration);

        IWorkflowBLLContext IBLLContextContainer<IWorkflowBLLContext>.Context => this.Workflow;
    }
}
