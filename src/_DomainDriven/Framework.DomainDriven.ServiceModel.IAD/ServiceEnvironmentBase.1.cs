using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Core.Services;

using JetBrains.Annotations;

namespace Framework.DomainDriven.ServiceModel.IAD
{
    public abstract class ServiceEnvironmentBase<TBLLContextContainer, TBLLContext> : ServiceEnvironmentBase
        where TBLLContextContainer : ServiceEnvironmentBase<TBLLContextContainer, TBLLContext>.ServiceEnvironmentBLLContextContainer
        where TBLLContext : ITypeResolverContainer<string>
    {
        protected ServiceEnvironmentBase(
            IServiceProvider serviceProvider,
            INotificationContext notificationContext,
            [NotNull] AvailableValues availableValues)
            : base(serviceProvider, notificationContext, availableValues)
        {
            this.ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; }

        protected virtual IEnumerable<IServiceEnvironmentModule<TBLLContextContainer>> GetModules()
        {
            yield break;
        }

        protected virtual IEnumerable<IDALListener> GetDALFlushedListeners(TBLLContextContainer container)
        {
            foreach (var module in this.GetModules())
            {
                foreach (var listener in module.GetDALFlushedListeners(container))
                {
                    yield return listener;
                }
            }
        }

        protected virtual IEnumerable<IDALListener> GetBeforeTransactionCompletedListeners(TBLLContextContainer container)
        {
            foreach (var module in this.GetModules())
            {
                foreach (var listener in module.GetBeforeTransactionCompletedListeners(container))
                {
                    yield return listener;
                }
            }

            if (container.Configuration.SubscriptionEnabled)
            {
                foreach (var listener in this.GetSubscriptionDALListeners(container))
                {
                    yield return listener;
                }
            }

            yield return this.GetFixDomainObjectEventRevisionNumberDALListener(container);
        }

        protected virtual IDALListener GetFixDomainObjectEventRevisionNumberDALListener(TBLLContextContainer container)
        {
            return new FixDomainObjectEventRevisionNumberDALListener(container.Configuration);
        }

        protected virtual IEnumerable<IDALListener> GetAfterTransactionCompletedListeners(TBLLContextContainer container)
        {
            yield break;
        }

        private IEnumerable<IDALListener> GetSubscriptionDALListeners(TBLLContextContainer container)
        {
            return from targetSystemService in container.Configuration.GetPersistentTargetSystemServices()

                   where targetSystemService.TargetSystem.SubscriptionEnabled

                   select new SubscriptionDALListener(targetSystemService, container.SubscriptionService);
        }

        public new abstract class ServiceEnvironmentBLLContextContainer : ServiceEnvironmentBase.ServiceEnvironmentBLLContextContainer, IServiceEnvironmentBLLContextContainer<TBLLContext>, IBLLContextContainer<TBLLContext>
        {
            private readonly ServiceEnvironmentBase<TBLLContextContainer, TBLLContext> serviceEnvironment;

            protected ServiceEnvironmentBLLContextContainer(
                    ServiceEnvironmentBase<TBLLContextContainer, TBLLContext> serviceEnvironment,
                    IServiceProvider scopedServiceProvider,
                    IDBSession session,
                    [NotNull] IUserAuthenticationService userAuthenticationService,
                    [NotNull] IDateTimeService dateTimeService,
                    SubscriptionMetadataStore subscriptionMetadataStore)
                : base(serviceEnvironment, scopedServiceProvider, session, userAuthenticationService, dateTimeService, subscriptionMetadataStore)
            {
                this.serviceEnvironment = serviceEnvironment;
                this.MainContext = LazyInterfaceImplementHelper.CreateProxy(this.CreateMainContext);
                this.SubscriptionService = LazyInterfaceImplementHelper.CreateProxy(this.CreateSubscriptionService);
            }

            public TBLLContext MainContext { get; }

            protected internal IStandardSubscriptionService SubscriptionService { get; }

            protected abstract TBLLContext CreateMainContext();

            protected override ITypeResolver<string> GetSecurityTypeResolver()
            {
                return this.MainContext.TypeResolver;
            }

            /// <summary>
            /// Вервис который будет отправлять подписки в шину или базу
            /// На текущий момент возможны две реализации:
            /// return new LocalDBSubscriptionService(this.Configuration); // Сохранение модификаций в локальную бд
            /// или
            /// return new DefaultActiveSubscriptionServiceClient(); // Отсылка модификаций в MSMQ
            /// Так же можно использовать либо TraceActiveSubscriptionService, либо EmptyActiveSubscriptionService
            /// </summary>
            /// <returns></returns>
            protected abstract IStandardSubscriptionService CreateSubscriptionService();

            #region IBLLContextContainer<TBLLContext> Members

            TBLLContext IBLLContextContainer<TBLLContext>.Context
            {
                get { return this.MainContext; }
            }

            #endregion

            /// <summary>
            /// Возврат DAL-подписчиков, при их вызове изменения в базе данных всё ещё доступны
            /// </summary>
            /// <returns></returns>
            protected override IEnumerable<IDALListener> GetBeforeTransactionCompletedListeners()
            {
                var dalListeners = new[]
                {
                    this.serviceEnvironment.GetBeforeTransactionCompletedListeners((TBLLContextContainer)this),
                    this.GetAuthorizationEventDALListeners(),
                    this.GetConfigurationEventDALListeners(),
                };

                return base.GetBeforeTransactionCompletedListeners().Concat(dalListeners.SelectMany());
            }

            /// <summary>
            /// Возврат DAL-подписчиков, при их вызове изменения в базе данных уже недоступны
            /// </summary>
            /// <returns></returns>
            protected override IEnumerable<IDALListener> GetAfterTransactionCompletedListeners()
            {
                return this.serviceEnvironment.GetAfterTransactionCompletedListeners((TBLLContextContainer)this);
            }

            protected override IEnumerable<IDALListener> GetDALFlushedListeners()
            {
                return this.serviceEnvironment.GetDALFlushedListeners((TBLLContextContainer)this);
            }
        }
    }
}
