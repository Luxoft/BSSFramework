﻿using System;
using System.Collections.Generic;
using Framework.Authorization.BLL;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.Persistent;
using Framework.Core.Services;
using Framework.Events;
using Framework.HierarchicalExpand;

using JetBrains.Annotations;

using ITargetSystemService = Framework.Configuration.BLL.ITargetSystemService;

namespace Framework.DomainDriven.ServiceModel.IAD
{
    public abstract class ServiceEnvironmentBase<TBLLContextContainer, TBLLContext, TPersistentDomainObjectBase, TAuditPersistentDomainObjectBase, TSecurityOperationCode> : ServiceEnvironmentBase<TBLLContextContainer, TBLLContext>

        where TBLLContextContainer : ServiceEnvironmentBase<TBLLContextContainer, TBLLContext, TPersistentDomainObjectBase, TAuditPersistentDomainObjectBase, TSecurityOperationCode>.ServiceEnvironmentBLLContextContainer

        where TBLLContext : class, IBLLBaseContextBase<TPersistentDomainObjectBase, Guid>,
                                   ISecurityServiceContainer<IRootSecurityService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>>,
                                   ISecurityBLLContext<IAuthorizationBLLContext, TPersistentDomainObjectBase, Guid>,
                                   IAccessDeniedExceptionServiceContainer<TPersistentDomainObjectBase>,
                                   ITypeResolverContainer<string>, IBLLOperationEventContext<TPersistentDomainObjectBase>
        where TPersistentDomainObjectBase : class, IDefaultIdentityObject
        where TAuditPersistentDomainObjectBase : class, TPersistentDomainObjectBase, IDefaultAuditPersistentDomainObjectBase

        where TSecurityOperationCode : struct, Enum
    {
        protected ServiceEnvironmentBase(
            IServiceProvider serviceProvider,
            INotificationContext notificationContext,
            [NotNull] AvailableValues availableValues,
            ISubscriptionMetadataFinder subscriptionsMetadataFinder = null)

            : base(serviceProvider, notificationContext, availableValues, subscriptionsMetadataFinder)
        {
        }


        public new abstract class ServiceEnvironmentBLLContextContainer : ServiceEnvironmentBase<TBLLContextContainer, TBLLContext>.ServiceEnvironmentBLLContextContainer
        {
            private readonly ServiceEnvironmentBase<TBLLContextContainer, TBLLContext, TPersistentDomainObjectBase, TAuditPersistentDomainObjectBase, TSecurityOperationCode> serviceEnvironment;

            private readonly Lazy<IEventsSubscriptionManager<TBLLContext, TPersistentDomainObjectBase>> lazyMainEventsSubscriptionManager;

            protected ServiceEnvironmentBLLContextContainer(
                    ServiceEnvironmentBase<TBLLContextContainer, TBLLContext, TPersistentDomainObjectBase, TAuditPersistentDomainObjectBase, TSecurityOperationCode> serviceEnvironment,
                    IServiceProvider scopedServiceProvider,
                    IDBSession session,
                    [NotNull] IUserAuthenticationService userAuthenticationService,
                    [NotNull] IDateTimeService dateTimeService)
                : base(serviceEnvironment, scopedServiceProvider, session, userAuthenticationService, dateTimeService)
            {
                this.serviceEnvironment = serviceEnvironment;

                this.lazyMainEventsSubscriptionManager = LazyHelper.Create(this.CreateMainEventsSubscriptionManager);
            }

            protected IEventsSubscriptionManager<TBLLContext, TPersistentDomainObjectBase> MainEventsSubscriptionManager => this.lazyMainEventsSubscriptionManager.Value;

            protected virtual ITypeResolver<string> SecurityObjectTypeResolver => this.MainContext.TypeResolver;


            protected virtual IEventsSubscriptionManager<TBLLContext, TPersistentDomainObjectBase> CreateMainEventsSubscriptionManager()
            {
                return null;
            }

            /// <inheritdoc />
            protected internal override void SubscribeEvents()
            {
                base.SubscribeEvents();

                this.MainEventsSubscriptionManager.Maybe(eventManager => eventManager.Subscribe());
            }

            protected override IAuthorizationExternalSource GetAuthorizationExternalSource()
            {
                return new AuthorizationExternalSource<TBLLContext, TPersistentDomainObjectBase, TAuditPersistentDomainObjectBase, TSecurityOperationCode>(this.MainContext, this.Authorization, this.SecurityObjectTypeResolver);
            }

            protected override IHierarchicalObjectExpanderFactory<Guid> GetHierarchicalObjectExpanderFactory()
            {
                return new HierarchicalObjectExpanderFactory<TPersistentDomainObjectBase, Guid>(this.MainContext.GetQueryableSource(), new ProjectionHierarchicalRealTypeResolver());
            }

            protected TBLLContext Impersonate(string principalName)
            {
                return this.serviceEnvironment.GetBLLContextContainer(this.ScopedServiceProvider, this.Session, principalName).MainContext;
            }

            protected override IEnumerable<ITargetSystemService> GetConfigurationTargetSystemServices(SubscriptionMetadataStore subscriptionMetadataStore)
            {
                yield return this.GetConfigurationConfigurationTargetSystemService();
            }

            protected Framework.Configuration.BLL.ITargetSystemService GetMainConfigurationTargetSystemService()
            {
                return new Framework.Configuration.BLL.TargetSystemService<TBLLContext, TPersistentDomainObjectBase>(
                    this.Configuration,
                    this.MainContext,
                    this.Configuration.Logics.TargetSystem.GetObjectBy(ts => ts.IsMain, true),
                    this.GetMainEventDALListeners(),
                    this.ServiceEnvironment.SubscriptionMetadataStore);
            }

            /// <inheritdoc />
            protected override IEnumerable<IDALListener> GetBeforeTransactionCompletedListeners()
            {
                foreach (var baseListener in base.GetBeforeTransactionCompletedListeners())
                {
                    yield return baseListener;
                }

                foreach (var listener in this.GetMainEventDALListeners())
                {
                    yield return listener;
                }
            }

            /// <summary>
            /// Получение DALListener-ов основной системы с возможностью ручных вызовов
            /// </summary>
            /// <returns></returns>
            protected virtual IEnumerable<IManualEventDALListener<TPersistentDomainObjectBase>> GetMainEventDALListeners()
            {
                yield break;
            }
        }
    }
}
