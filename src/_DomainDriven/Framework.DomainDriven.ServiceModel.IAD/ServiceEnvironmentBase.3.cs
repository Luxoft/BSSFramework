using System;
using System.Collections.Generic;

using Framework.Authorization.BLL;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.Core.Services;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.BLL.Security.Lock;
using Framework.DomainDriven.BLL.Tracking;
using Framework.Persistent;

namespace Framework.DomainDriven.ServiceModel.IAD
{
    public abstract class ServiceEnvironmentBase<TBLLContextContainer, TBLLContext, TPersistentDomainObjectBase, TAuditPersistentDomainObjectBase, TSecurityOperationCode, TNamedLockObject, TNamedLockOperation>
        : ServiceEnvironmentBase<TBLLContextContainer, TBLLContext, TPersistentDomainObjectBase, TAuditPersistentDomainObjectBase, TSecurityOperationCode>

        where TBLLContextContainer : ServiceEnvironmentBase<TBLLContextContainer, TBLLContext, TPersistentDomainObjectBase, TAuditPersistentDomainObjectBase, TSecurityOperationCode>.ServiceEnvironmentBLLContextContainer
        where TBLLContext : class, IBLLBaseContextBase<TPersistentDomainObjectBase, Guid>,
                                   ISecurityServiceContainer<IRootSecurityService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>>,
                                   ISecurityBLLContext<IAuthorizationBLLContext, TPersistentDomainObjectBase, Guid>,
                                   IAccessDeniedExceptionServiceContainer<TPersistentDomainObjectBase>,
                                   ITypeResolverContainer<string>,
                                   ITrackingServiceContainer<TPersistentDomainObjectBase>,
                                   IBLLOperationEventContext<TPersistentDomainObjectBase>
        where TPersistentDomainObjectBase : class, IDefaultIdentityObject
        where TSecurityOperationCode : struct, Enum

        where TNamedLockObject : class, TPersistentDomainObjectBase, INamedLock<TNamedLockOperation>
        where TNamedLockOperation : struct, Enum
        where TAuditPersistentDomainObjectBase : class, TPersistentDomainObjectBase, IDefaultAuditPersistentDomainObjectBase
    {
        protected ServiceEnvironmentBase(
            IServiceProvider serviceProvider,
            IDBSessionFactory sessionFactory,
            INotificationContext notificationContext,
            IUserAuthenticationService userAuthenticationService,
            IMessageSender<RunRegularJobModel> regularJobSender = null,
            ISubscriptionMetadataFinder subscriptionsMetadataFinder = null)
            : base(serviceProvider, sessionFactory, notificationContext, userAuthenticationService, regularJobSender, subscriptionsMetadataFinder)
        {
        }

        protected override IEnumerable<IDALListener> GetBeforeTransactionCompletedListeners(TBLLContextContainer container)
        {
            foreach (var listener in base.GetBeforeTransactionCompletedListeners(container))
            {
                yield return listener;
            }

            yield return new DenormalizeHierarchicalDALListener<TBLLContext, TPersistentDomainObjectBase, TNamedLockObject, TNamedLockOperation>(container.MainContext);
        }
    }
}
