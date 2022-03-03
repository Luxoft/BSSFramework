using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Core.Services;

namespace Framework.DomainDriven.ServiceModel.IAD
{
    public interface IBLLContextContainerModule
    {
        IEnumerable<IDALListener> GetBeforeTransactionCompletedListeners();
    }

    public abstract partial class ServiceEnvironmentBase<TBLLContextContainer, TBLLContext> : ServiceEnvironmentBase, IRootServiceEnvironment<TBLLContext, TBLLContextContainer>
        where TBLLContextContainer : ServiceEnvironmentBase<TBLLContextContainer, TBLLContext>.ServiceEnvironmentBLLContextContainer
        where TBLLContext : ITypeResolverContainer<string>
    {
        protected ServiceEnvironmentBase(
            IServiceProvider serviceProvider,
            IDBSessionFactory sessionFactory,
            INotificationContext notificationContext,
            IUserAuthenticationService userAuthenticationService,
            ISubscriptionMetadataFinder subscriptionsMetadataFinder = null)
            : base(serviceProvider, sessionFactory, notificationContext, userAuthenticationService, subscriptionsMetadataFinder)
        {
            this.ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; }

        protected virtual IEnumerable<IBLLContextContainerModule> GetModules()
        {
            yield break;
        }

        protected virtual IEnumerable<IDALListener> GetDALFlushedListeners(TBLLContextContainer container)
        {
            yield break;
        }

        protected virtual IEnumerable<IDALListener> GetBeforeTransactionCompletedListeners(TBLLContextContainer container)
        {
            foreach (var listener in this.GetAttachmentCleanerDALListeners(container))
            {
                yield return listener;
            }

            if (container.Configuration.SubscriptionEnabled)
            {
                foreach (var listener in this.GetSubscriptionDALListeners(container))
                {
                    yield return listener;
                }
            }

            yield return this.GetFixDomainObjectEventRevisionNumberDALListener(container);

            foreach (var module in this.GetModules())
            {
                foreach (var listener in module.GetBeforeTransactionCompletedListeners())
                {
                    yield return listener;
                }
            }
        }

        protected virtual IDALListener GetFixDomainObjectEventRevisionNumberDALListener(TBLLContextContainer container)
        {
            return new FixDomainObjectEventRevisionNumberDALListener(container.Configuration);
        }

        protected virtual IEnumerable<IDALListener> GetAfterTransactionCompletedListeners(TBLLContextContainer container)
        {
            yield break;
        }


        private IEnumerable<IDALListener> GetAttachmentCleanerDALListeners(TBLLContextContainer container)
        {
            return from targetSystemService in container.Configuration.GetPersistentTargetSystemServices()

                   where targetSystemService.HasAttachments

                   select new AttachmentCleanerDALListener(targetSystemService);
        }

        private IEnumerable<IDALListener> GetSubscriptionDALListeners(TBLLContextContainer container)
        {
            return from targetSystemService in container.Configuration.GetPersistentTargetSystemServices()

                   where targetSystemService.TargetSystem.SubscriptionEnabled

                   select new SubscriptionDALListener(targetSystemService, container.SubscriptionService);
        }

        IBLLContextContainer<TBLLContext> IServiceEnvironment<TBLLContext>.GetBLLContextContainer(IServiceProvider scopedServiceProvider, IDBSession session, string currentPrincipalName)
        {
            return this.GetBLLContextContainer(scopedServiceProvider, session, currentPrincipalName);
        }

        public IContextEvaluator<TBLLContext> GetContextEvaluator(
            IServiceProvider currentScopedServiceProvider = null)
        {
            return currentScopedServiceProvider == null
                       ? new RootContextEvaluator<TBLLContext>(this, this.RootServiceProvider)
                       : new ScopeContextEvaluator<TBLLContext>(this, currentScopedServiceProvider);
        }

        public TBLLContextContainer GetBLLContextContainer(IServiceProvider scopedServiceProvider, IDBSession session, string currentPrincipalName = null)
        {
            var container = this.CreateBLLContextContainer(scopedServiceProvider, session, currentPrincipalName);

            if (!this.IsInitialize)
            {
                container.SubscribeEvents();
            }

            return container;
        }

        protected abstract TBLLContextContainer CreateBLLContextContainer(IServiceProvider scopedServiceProvider, IDBSession session, string currentPrincipalName = null);

        public sealed override ServiceEnvironmentBase.ServiceEnvironmentBLLContextContainer GetBLLContextContainerBase(IServiceProvider scopedServiceProvider, IDBSession session, string currentPrincipalName = null)
        {
            return this.GetBLLContextContainer(scopedServiceProvider, session, currentPrincipalName);
        }

        public new abstract class ServiceEnvironmentBLLContextContainer : ServiceEnvironmentBase.ServiceEnvironmentBLLContextContainer, IServiceEnvironmentBLLContextContainer<TBLLContext>, IBLLContextContainer<TBLLContext>
        {
            private readonly ServiceEnvironmentBase<TBLLContextContainer, TBLLContext> serviceEnvironment;

            protected ServiceEnvironmentBLLContextContainer(ServiceEnvironmentBase<TBLLContextContainer, TBLLContext> serviceEnvironment, IServiceProvider scopedServiceProvider, IDBSession session, string currentPrincipalName)
                : base(serviceEnvironment, scopedServiceProvider, session, currentPrincipalName)
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

                return dalListeners.SelectMany();
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
