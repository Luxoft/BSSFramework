﻿using System.Linq.Expressions;

using Framework.Authorization.Notification;
using Framework.Events;
using Framework.Persistent;
using Framework.SecuritySystem.DependencyInjection;

namespace Framework.DomainDriven.Setup;

public interface IBssFrameworkSettings
{
    bool RegisterBaseNamedLockTypes { get; set; }

    bool RegisterDenormalizeHierarchicalDALListener { get; set; }

    IBssFrameworkSettings AddSecuritySystem(Action<ISecuritySystemSettings> settings);

    IBssFrameworkSettings AddNamedLockType(Type namedLockType);

    IBssFrameworkSettings AddListener<TListener>(bool registerSelf = false)
        where TListener : class, IDALListener;

    IBssFrameworkSettings AddExtensions(IBssFrameworkExtension extension);

    IBssFrameworkSettings SetNotificationPrincipalExtractor<T>()
        where T : INotificationPrincipalExtractor;

    IBssFrameworkSettings SetDomainObjectEventMetadata<T>()
        where T : IDomainObjectEventMetadata;

    IBssFrameworkSettings SetPrincipalIdentitySource<TDomainObject>(Expression<Func<TDomainObject, string>> namePath)
        where TDomainObject : IIdentityObject<Guid>;
}
