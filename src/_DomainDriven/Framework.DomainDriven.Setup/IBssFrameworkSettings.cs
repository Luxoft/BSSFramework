using Framework.DomainDriven._Visitors;
using Framework.DomainDriven.Lock;
using Framework.DomainDriven.NHibernate;
using Framework.Events;
using Framework.SecuritySystem.DependencyInjection;

using nuSpec.Abstraction;

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

    IBssFrameworkSettings SetSpecificationEvaluator<TSpecificationEvaluator>()
        where TSpecificationEvaluator : class, ISpecificationEvaluator;

    IBssFrameworkSettings AddDatabaseVisitors<TExpressionVisitorContainerItem>(bool scoped = false)
        where TExpressionVisitorContainerItem : class, IExpressionVisitorContainerItem;

    IBssFrameworkSettings AddDatabaseSettings(Action<INHibernateSetupObject> setupAction);
}
