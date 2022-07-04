using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Framework.Authorization;
using Framework.Authorization.BLL;
using Framework.Configuration.BLL;
using Framework.Configuration.BLL.Notification;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Core;
using Framework.Core.Serialization;
using Framework.Core.Services;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.SecuritySystem.Rules.Builders;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.Events;
using Framework.HierarchicalExpand;
using Framework.Notification;
using Framework.Notification.DTO;
using Framework.Notification.New;
using Framework.Persistent;
using Framework.QueryLanguage;
using Framework.Report;
using Framework.SecuritySystem;
using Framework.Validation;

using JetBrains.Annotations;

namespace Framework.DomainDriven.ServiceModel.IAD
{
    public abstract partial class ServiceEnvironmentBase :
        IAuthorizationServiceEnvironment,
        IConfigurationServiceEnvironment,
        IDisposable
    {
        public readonly IFetchService<Framework.Authorization.Domain.PersistentDomainObjectBase, FetchBuildRule> AuthorizationFetchService;

        public readonly IFetchService<Framework.Configuration.Domain.PersistentDomainObjectBase, FetchBuildRule> ConfigurationFetchService;

        public readonly IUserAuthenticationService UserAuthenticationService;

        protected ServiceEnvironmentBase(
            [NotNull] IServiceProvider serviceProvider,
            [NotNull] IDBSessionFactory sessionFactory,
            [NotNull] INotificationContext notificationContext,
            [NotNull] IUserAuthenticationService userAuthenticationService,
            ISubscriptionMetadataFinder subscriptionsMetadataFinder = null)
        {
            this.RootServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.SessionFactory = sessionFactory ?? throw new ArgumentNullException(nameof(sessionFactory));

            this.NotificationContext = notificationContext ?? throw new ArgumentNullException(nameof(notificationContext));

            this.ObjectStorage = new TimeoutStorage(DateTimeService.Default, new TimeSpan(0, 0, 10, 0));

            this.DefaultAuthorizationValidatorCompileCache =

                this.SessionFactory
                    .AvailableValues
                    .ToValidation()
                    .ToBLLContextValidationExtendedData<IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase, Guid>()
                    .Pipe(extendedValidationData => new AuthorizationValidationMap(extendedValidationData))
                    .ToCompileCache();


            this.DefaultConfigurationValidatorCompileCache =

                this.SessionFactory
                    .AvailableValues
                    .ToValidation()
                    .ToBLLContextValidationExtendedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase, Guid>()
                    .Pipe(extendedValidationData => new ConfigurationValidationMap(extendedValidationData))
                    .ToCompileCache();

            this.AuthorizationFetchService = new AuthorizationMainFetchService().WithCompress().WithCache().WithLock().Add(FetchService<Framework.Authorization.Domain.PersistentDomainObjectBase>.OData);

            this.ConfigurationFetchService = new ConfigurationMainFetchService().WithCompress().WithCache().WithLock().Add(FetchService<Framework.Configuration.Domain.PersistentDomainObjectBase>.OData);

            this.SubscriptionMetadataStore = new SubscriptionMetadataStore(subscriptionsMetadataFinder ?? new SubscriptionMetadataFinder());

            this.UserAuthenticationService = userAuthenticationService ?? throw new ArgumentNullException(nameof(userAuthenticationService));
        }

        /// <summary>
        /// Кеш валидатора для авторизации
        /// </summary>
        protected ValidatorCompileCache DefaultAuthorizationValidatorCompileCache { get; }

        /// <summary>
        /// Кеш валидатора для воркфлоу
        /// </summary>
        protected ValidatorCompileCache DefaultConfigurationValidatorCompileCache { get; }

        /// <summary>
        /// Провайдер используется для создания scope в интеграционных тестах
        /// </summary>
        public IServiceProvider RootServiceProvider { get; }

        /// <summary>
        /// Gets the notification context.
        /// </summary>
        /// <value>
        /// The notification context.
        /// </value>
        public INotificationContext NotificationContext { get; }

        public IDBSessionFactory SessionFactory { get; }

        public virtual bool IsDebugMode => Debugger.IsAttached;

        /// <summary>
        /// Флаг, указывающий, что происходит инициализация системы (в этом состояния отключены подписки на все евенты)
        /// </summary>
        public bool IsInitialize { get; private set; }

        public IObjectStorage ObjectStorage { get; private set; }

        /// <summary>
        /// Получает хранилище описаний подписок.
        /// </summary>
        /// <value>
        /// Хранилище описаний подписок.
        /// </value>
        public SubscriptionMetadataStore SubscriptionMetadataStore { get; }

        public abstract ServiceEnvironmentBLLContextContainer GetBLLContextContainerBase(IServiceProvider serviceProvider, IDBSession session, string currentPrincipalName = null);

        #region IServiceEnvironment<AuthorizationBLLContext> Members

        IBLLContextContainer<IAuthorizationBLLContext> IServiceEnvironment<IAuthorizationBLLContext>.GetBLLContextContainer(IServiceProvider serviceProvider, IDBSession session, string currentPrincipalName)
        {
            return this.GetBLLContextContainerBase(serviceProvider, session, currentPrincipalName);
        }

        #endregion

        #region IServiceEnvironment<ConfigurationBLLContext> Members

        IBLLContextContainer<IConfigurationBLLContext> IServiceEnvironment<IConfigurationBLLContext>.GetBLLContextContainer(IServiceProvider serviceProvider, IDBSession session, string currentPrincipalName)
        {
            return this.GetBLLContextContainerBase(serviceProvider, session, currentPrincipalName);
        }

        #endregion

        protected void InitializeOperation(Action operation)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));

            if (this.IsInitialize)
            {
                throw new Exception("already initializing");
            }
            else
            {
                this.IsInitialize = true;

                try
                {
                    operation();
                }
                finally
                {
                    this.IsInitialize = false;
                }
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.SessionFactory.Dispose();
        }

        public abstract class ServiceEnvironmentBLLContextContainer :

            IServiceEnvironmentBLLContextContainer,

            IBLLContextContainer<IAuthorizationBLLContext>,
            IBLLContextContainer<IConfigurationBLLContext>
        {
            protected readonly ServiceEnvironmentBase ServiceEnvironment;

            private readonly string currentPrincipalName;

            private readonly IEnumerable<Framework.Configuration.BLL.ITargetSystemService> targetSystems;



            protected readonly BLLOperationEventListenerContainer<Framework.Authorization.Domain.DomainObjectBase>
                    AuthorizationOperationListeners =
                            new BLLOperationEventListenerContainer<Framework.Authorization.Domain.DomainObjectBase>();

            protected readonly BLLSourceEventListenerContainer<Framework.Authorization.Domain.PersistentDomainObjectBase>
                    AuthorizationSourceListeners =
                            new BLLSourceEventListenerContainer<Framework.Authorization.Domain.PersistentDomainObjectBase>();


            protected readonly BLLOperationEventListenerContainer<Framework.Configuration.Domain.DomainObjectBase>
                    ConfigurationOperationListeners =
                            new BLLOperationEventListenerContainer<Framework.Configuration.Domain.DomainObjectBase>();

            protected readonly BLLSourceEventListenerContainer<Framework.Configuration.Domain.PersistentDomainObjectBase>
                    ConfigurationSourceListeners =
                            new BLLSourceEventListenerContainer<Framework.Configuration.Domain.PersistentDomainObjectBase>();


            private readonly Lazy<IEventsSubscriptionManager<IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase>> lazyAuthorizationEventsSubscriptionManager;

            private readonly Lazy<IEventsSubscriptionManager<IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase>> lazyConfigurationEventsSubscriptionManager;


            protected ServiceEnvironmentBLLContextContainer([NotNull] ServiceEnvironmentBase serviceEnvironment, [NotNull] IServiceProvider scopedServiceProvider, [NotNull] IDBSession session, string currentPrincipalName)
            {
                this.ServiceEnvironment = serviceEnvironment ?? throw new ArgumentNullException(nameof(serviceEnvironment));
                this.ScopedServiceProvider = scopedServiceProvider ?? throw new ArgumentNullException(nameof(scopedServiceProvider));
                this.Session = session ?? throw new ArgumentNullException(nameof(session));

                this.currentPrincipalName = currentPrincipalName;

                this.StandartExpressionBuilder = LazyInterfaceImplementHelper.CreateProxy(this.GetStandartExpressionBuilder);

                this.NotificationService = LazyInterfaceImplementHelper.CreateProxy(this.CreateNotificationService);

                this.targetSystems = LazyHelper.Create(() => this.GetConfigurationTargetSystemServices(serviceEnvironment.SubscriptionMetadataStore)).Unwrap();

                this.Authorization = LazyInterfaceImplementHelper.CreateProxy(this.CreateAuthorizationBLLContext);

                this.Configuration = LazyInterfaceImplementHelper.CreateProxy(this.CreateConfigurationBLLContext);

                this.HierarchicalObjectExpanderFactory = LazyInterfaceImplementHelper.CreateProxy(this.GetHierarchicalObjectExpanderFactory);

                this.SystemConstantSerializerFactory = SerializerFactory.Default;

                this.lazyAuthorizationEventsSubscriptionManager = LazyHelper.Create(this.CreateAuthorizationEventsSubscriptionManager);

                this.lazyConfigurationEventsSubscriptionManager = LazyHelper.Create(this.CreateConfigurationEventsSubscriptionManager);
            }

            public IServiceProvider ScopedServiceProvider { get; }


            public IAuthorizationBLLContext Authorization { get; }

            public IConfigurationBLLContext Configuration { get; }

            public IDBSession Session { get; }


            protected IEventsSubscriptionManager<IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase> AuthorizationEventsSubscriptionManager => this.lazyAuthorizationEventsSubscriptionManager.Value;

            protected IEventsSubscriptionManager<IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase> ConfigurationEventsSubscriptionManager => this.lazyConfigurationEventsSubscriptionManager.Value;

            #region IBLLContextContainer<AuthorizationBLLContext> Members

            IAuthorizationBLLContext IBLLContextContainer<IAuthorizationBLLContext>.Context
            {
                get { return this.Authorization; }
            }

            #endregion

            #region IBLLContextContainer<ConfigurationBLLContext> Members

            IConfigurationBLLContext IBLLContextContainer<IConfigurationBLLContext>.Context
            {
                get { return this.Configuration; }
            }

            #endregion

            protected INotificationService NotificationService { get; }

            public IStandartExpressionBuilder StandartExpressionBuilder { get; }

            public IHierarchicalObjectExpanderFactory<Guid> HierarchicalObjectExpanderFactory { get; }

            protected virtual ISerializerFactory<string> SystemConstantSerializerFactory { get; }

            protected virtual IEnumerable<IBLLContextContainerModule> GetModules()
            {
                yield break;
            }

            protected virtual IAuthorizationBLLContext CreateAuthorizationBLLContext()
            {
                return new AuthorizationBLLContext(
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
                    LazyInterfaceImplementHelper.CreateProxy(this.GetSecurityTypeResolver));
            }

            protected virtual IConfigurationBLLContext CreateConfigurationBLLContext()
            {
                return new ConfigurationBLLContext(
                    this.ScopedServiceProvider,
                    this.Session.GetDALFactory<Framework.Configuration.Domain.PersistentDomainObjectBase, Guid>(),
                    this.ConfigurationOperationListeners,
                    this.ConfigurationSourceListeners,
                    this.Session.GetObjectStateService(),
                    this.GetAccessDeniedExceptionService<Framework.Configuration.Domain.PersistentDomainObjectBase, Guid>(),
                    this.StandartExpressionBuilder,
                    LazyInterfaceImplementHelper.CreateProxy<IValidator>(this.CreateConfigurationValidator),
                    this.HierarchicalObjectExpanderFactory,
                    this.ServiceEnvironment.ConfigurationFetchService,
                    this.GetDateTimeService(),
                    LazyInterfaceImplementHelper.CreateProxy(() => this.GetSecurityExpressionBuilderFactory<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase, Guid>(this.Configuration)),
                    this.NotificationService.ExceptionSender,
                    this.NotificationService.SubscriptionSender,
                    () => new ConfigurationSecurityService(this.Configuration),
                    () => new ConfigurationBLLFactoryContainer(this.Configuration),
                    this.Authorization,
                    this.GetEmployeeSource,
                    this.targetSystems,
                    this.SystemConstantSerializerFactory,
                    LazyInterfaceImplementHelper.CreateProxy(this.GetExceptionService),
                    this.Session.GetCurrentRevision);
            }


            /// <summary>
            /// Создание валидатора для авторизации
            /// </summary>
            /// <returns></returns>
            protected virtual AuthorizationValidator CreateAuthorizationValidator()
            {
                return new AuthorizationValidator(this.Authorization, this.ServiceEnvironment.DefaultAuthorizationValidatorCompileCache);
            }

            /// <summary>
            /// Создание валидатора для утилит
            /// </summary>
            /// <returns></returns>
            protected virtual ConfigurationValidator CreateConfigurationValidator()
            {
                return new ConfigurationValidator(this.Configuration, this.ServiceEnvironment.DefaultConfigurationValidatorCompileCache);
            }

            /// <summary>
            /// Подписка на евенты
            /// </summary>
            protected internal virtual void SubscribeEvents()
            {
                this.Session.Flushed += (_, eventArgs) =>
                {
                    var listeners = this.GetDALFlushedListeners().ToArray();

                    listeners.Foreach(listener => listener.Process(eventArgs));
                };

                this.Session.BeforeTransactionCompleted += (_, eventArgs) =>
                {
                    var listeners = this.GetBeforeTransactionCompletedListeners().ToArray();

                    listeners.Foreach(listener => listener.Process(eventArgs));
                };

                this.Session.AfterTransactionCompleted += (_, eventArgs) =>
                {
                    var listeners = this.GetAfterTransactionCompletedListeners().ToArray();

                    listeners.Foreach(listener => listener.Process(eventArgs));
                };

                this.AuthorizationEventsSubscriptionManager.Maybe(eventManager => eventManager.Subscribe());

                this.ConfigurationEventsSubscriptionManager.Maybe(eventManager => eventManager.Subscribe());

                foreach (var module in this.GetModules())
                {
                    module.SubscribeEvents();
                }
            }

            protected virtual IEnumerable<IDALListener> GetBeforeTransactionCompletedListeners()
            {
                foreach (var module in this.GetModules())
                {
                    foreach (var listener in module.GetBeforeTransactionCompletedListeners())
                    {
                        yield return listener;
                    }
                }
            }

            protected abstract IEnumerable<IDALListener> GetAfterTransactionCompletedListeners();

            [Obsolete("Use GetBeforeTransactionCompletedListeners or GetAfterTransactionCompletedListeners methods", true)]
            protected virtual IEnumerable<IDALListener> GetTransactionCompletedListeners()
            {
                throw new NotImplementedException();
            }

            protected abstract IEnumerable<IDALListener> GetDALFlushedListeners();


            protected virtual IEventsSubscriptionManager<IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase> CreateAuthorizationEventsSubscriptionManager()
            {
                return null;
            }

            protected virtual IEventsSubscriptionManager<IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase> CreateConfigurationEventsSubscriptionManager()
            {
                return null;
            }

            public virtual ISecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> GetSecurityExpressionBuilderFactory<TBLLContext, TPersistentDomainObjectBase, TIdent>(TBLLContext context)
                where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
                where TBLLContext : class, ISecurityBLLContext<IAuthorizationBLLContext<TIdent>, TPersistentDomainObjectBase, TIdent>, IHierarchicalObjectExpanderFactoryContainer<TIdent>
            {
                return new Framework.SecuritySystem.Rules.Builders.MaterializedPermissions.SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent>(context.HierarchicalObjectExpanderFactory, context.Authorization);
            }

            protected virtual INotificationService CreateNotificationService()
            {
                var templateSender = this.GetMainTemplateSender().ToMessageTemplateSender(this.Configuration, this.ServiceEnvironment.NotificationContext.Sender);

                var notificationSender = this.GetMainTemplateSender().ToNotificationSender(this.Configuration, this.ServiceEnvironment.NotificationContext.Sender);

                var subscriptionMessageSender = this.GetSubscriptionTemplateSender().ToMessageTemplateSender(this.Configuration, this.ServiceEnvironment.NotificationContext.Sender);

                var exceptionSender = this.GetExceptionSender();

                return new NotificationService(templateSender, notificationSender, subscriptionMessageSender, exceptionSender);
            }

            /// <summary>
            /// Создаёт экземпляр класса, который рассылает уведомления об исключениях.
            /// </summary>
            /// <returns>Экземпляр класса, который рассылает уведомления об исключениях.</returns>
            protected virtual IMessageSender<Exception> GetExceptionSender()
            {
                return SmtpMessageSender.Configuration.ToExceptionSender(
                    this.Configuration,
                    this.ServiceEnvironment.NotificationContext.Sender,
                    this.ServiceEnvironment.NotificationContext.ExceptionReceivers);
            }

            public virtual IDateTimeService GetDateTimeService()
            {
                return DateTimeService.Default;
            }

            protected virtual IUserAuthenticationService GetUserAuthenticationService()
            {
                if (string.IsNullOrWhiteSpace(this.currentPrincipalName))
                {
                    return this.ServiceEnvironment.UserAuthenticationService;
                }

                return Core.Services.UserAuthenticationService.CreateFor(this.currentPrincipalName);
            }

            protected virtual IMessageSender<NotificationEventDTO> GetMainTemplateSender()
            {
                return this.GetMessageTemplateSender();
            }

            protected virtual IMessageSender<NotificationEventDTO> GetSubscriptionTemplateSender()
            {
                return this.GetMessageTemplateSender();
            }

            protected virtual IMessageSender<NotificationEventDTO> GetMessageTemplateSender()
            {
                ////return new LocalDBNotificationEventDTOMessageSender(this.Configuration); // Сохранение нотификаций в локальной бд, откуда их будет забирать Biztalk
                return this.ServiceEnvironment.NotificationContext.MSMQNotificationMessageSender; // Отсылка нотификаций в Biztalk через MSMQ
            }

            protected virtual IStandartExpressionBuilder GetStandartExpressionBuilder()
            {
                return Framework.QueryLanguage.StandartExpressionBuilder.Default;
            }

            /// <summary>
            /// Получение сервиса обрабоки исключений
            /// </summary>
            /// <returns></returns>
            protected virtual IExceptionService GetExceptionService()
            {
                return new ExceptionService(this.Configuration);
            }

            protected virtual IEnumerable<Framework.Configuration.BLL.ITargetSystemService> GetConfigurationTargetSystemServices(
                SubscriptionMetadataStore subscriptionMetadataStore)
            {
                yield break;
            }

            protected abstract ITypeResolver<string> GetSecurityTypeResolver();


            protected abstract IAuthorizationExternalSource GetAuthorizationExternalSource();

            protected abstract IBLLSimpleQueryBase<IEmployee> GetEmployeeSource(BLLSecurityMode securityMode);

            protected abstract IHierarchicalObjectExpanderFactory<Guid> GetHierarchicalObjectExpanderFactory();

            public virtual IAccessDeniedExceptionService<TPersistentDomainObjectBase> GetAccessDeniedExceptionService<TPersistentDomainObjectBase, TIdent>()
                where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
            {
                return new AccessDeniedExceptionService<TPersistentDomainObjectBase, TIdent>();
            }

            protected Framework.Configuration.BLL.ITargetSystemService GetConfigurationConfigurationTargetSystemService()
            {
                return new Framework.Configuration.BLL.TargetSystemService<IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase>(
                    this.Configuration,
                    this.Configuration,
                    this.Configuration.Logics.TargetSystem.GetByName(TargetSystemHelper.ConfigurationName, true),
                    this.GetConfigurationEventDALListeners(),
                    this.ServiceEnvironment.SubscriptionMetadataStore);
            }

            protected Framework.Configuration.BLL.ITargetSystemService GetAuthorizationConfigurationTargetSystemService()
            {
                return new Framework.Configuration.BLL.TargetSystemService<IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase>(
                    this.Configuration,
                    this.Authorization,
                    this.Configuration.Logics.TargetSystem.GetByName(TargetSystemHelper.AuthorizationName, true),
                    this.GetAuthorizationEventDALListeners(),
                    this.ServiceEnvironment.SubscriptionMetadataStore);
            }

            public ServiceEnvironmentBLLContextContainer Impersonate(string principalName)
            {
                return this.ServiceEnvironment.GetBLLContextContainerBase(this.ScopedServiceProvider, this.Session, principalName);
            }

            /// <summary>
            /// Получение авторизацонных DALListener-ов с возможностью ручных вызовов
            /// </summary>
            /// <returns></returns>
            protected virtual IEnumerable<IManualEventDALListener<Framework.Authorization.Domain.PersistentDomainObjectBase>> GetAuthorizationEventDALListeners()
            {
                yield break;
            }

            /// <summary>
            /// Получение конфигурационных DALListener-ов с возможностью ручных вызовов
            /// </summary>
            /// <returns></returns>
            protected virtual IEnumerable<IManualEventDALListener<Framework.Configuration.Domain.PersistentDomainObjectBase>> GetConfigurationEventDALListeners()
            {
                yield break;
            }
        }
    }
}
