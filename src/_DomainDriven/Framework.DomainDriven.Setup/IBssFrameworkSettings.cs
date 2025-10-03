using Framework.DomainDriven._Visitors;
using Framework.DomainDriven.Lock;
using Framework.Events;

using SecuritySystem.DependencyInjection;

namespace Framework.DomainDriven.Setup;

public interface IBssFrameworkSettings : IBssFrameworkSettingsBase<IBssFrameworkSettings>
{
    bool RegisterDenormalizeHierarchicalDALListener { get; set; }

    IBssFrameworkSettings AddSecuritySystem(Action<ISecuritySystemSettings> setupAction);

    IBssFrameworkSettings AddNamedLocks(Action<IGenericNamedLockSetup> setupAction);

    IBssFrameworkSettings AddListener<TListener>()
        where TListener : class, IDALListener;

    IBssFrameworkSettings SetDomainObjectEventMetadata<T>()
        where T : IDomainObjectEventMetadata;

    IBssFrameworkSettings AddQueryVisitors<TExpressionVisitorContainerItem>(bool scoped = false)
        where TExpressionVisitorContainerItem : class, IExpressionVisitorContainerItem;
}
