using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Framework.Authorization.BLL;
using Framework.Configuration.BLL;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.Events;

using JetBrains.Annotations;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ServiceModel.IAD
{
    public abstract class ServiceEnvironmentBase : IServiceEnvironment
    {
        protected ServiceEnvironmentBase(
            [NotNull] IServiceProvider serviceProvider,
            [NotNull] INotificationContext notificationContext)
        {
            this.RootServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.NotificationContext = notificationContext ?? throw new ArgumentNullException(nameof(notificationContext));

        }

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

        public virtual bool IsDebugMode => Debugger.IsAttached;

        /// <summary>
        /// Флаг, указывающий, что происходит инициализация системы (в этом состояния отключены подписки на все евенты)
        /// </summary>
        public bool IsInitialize { get; private set; }

        public void InitializeOperation(Action operation)
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

        public abstract class ServiceEnvironmentBLLContextContainer :

            IServiceEnvironmentBLLContextContainer,

            IBLLContextContainer<IAuthorizationBLLContext>,
            IBLLContextContainer<IConfigurationBLLContext>
        {
            protected readonly ServiceEnvironmentBase ServiceEnvironment;


            private readonly Lazy<IEventsSubscriptionManager<IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase>> lazyAuthorizationEventsSubscriptionManager;

            private readonly Lazy<IEventsSubscriptionManager<IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase>> lazyConfigurationEventsSubscriptionManager;


            protected ServiceEnvironmentBLLContextContainer(
                    [NotNull] ServiceEnvironmentBase serviceEnvironment,
                    [NotNull] IServiceProvider scopedServiceProvider,
                    [NotNull] IDBSession session)
            {
                this.ServiceEnvironment = serviceEnvironment ?? throw new ArgumentNullException(nameof(serviceEnvironment));
                this.ScopedServiceProvider = scopedServiceProvider ?? throw new ArgumentNullException(nameof(scopedServiceProvider));
                this.Session = session ?? throw new ArgumentNullException(nameof(session));

                this.lazyAuthorizationEventsSubscriptionManager = LazyHelper.Create(this.CreateAuthorizationEventsSubscriptionManager);

                this.lazyConfigurationEventsSubscriptionManager = LazyHelper.Create(this.CreateConfigurationEventsSubscriptionManager);
            }

            public IServiceProvider ScopedServiceProvider { get; }


            public IAuthorizationBLLContext Authorization => this.ScopedServiceProvider.GetRequiredService<IAuthorizationBLLContext>();

            public IConfigurationBLLContext Configuration => this.ScopedServiceProvider.GetRequiredService<IConfigurationBLLContext>();

            public IDBSession Session { get; }


            protected IEventsSubscriptionManager<IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase> AuthorizationEventsSubscriptionManager => this.lazyAuthorizationEventsSubscriptionManager.Value;

            protected IEventsSubscriptionManager<IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase> ConfigurationEventsSubscriptionManager => this.lazyConfigurationEventsSubscriptionManager.Value;

            #region IBLLContextContainer<AuthorizationBLLContext> Members

            IAuthorizationBLLContext IBLLContextContainer<IAuthorizationBLLContext>.Context => this.Authorization;

            #endregion

            #region IBLLContextContainer<ConfigurationBLLContext> Members

            IConfigurationBLLContext IBLLContextContainer<IConfigurationBLLContext>.Context => this.Configuration;

            #endregion

            protected virtual IEnumerable<IBLLContextContainerModule> GetModules()
            {
                yield break;
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
