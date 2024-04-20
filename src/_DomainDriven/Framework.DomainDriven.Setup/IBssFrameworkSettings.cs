using Framework.Authorization.Notification;
using Framework.Events;
using Framework.Persistent;

using Framework.SecuritySystem;
using Framework.SecuritySystem.DependencyInjection;
using Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

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
}
