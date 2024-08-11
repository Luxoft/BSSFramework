using System.Linq.Expressions;

using Framework.Authorization.Notification;
using Framework.DomainDriven._Visitors;
using Framework.DomainDriven.NHibernate;
using Framework.Events;
using Framework.Persistent;
using Framework.SecuritySystem;
using Framework.SecuritySystem.DependencyInjection;

using nuSpec.Abstraction;

namespace Framework.DomainDriven.Setup;

public interface IBssFrameworkSettings
{
    bool RegisterBaseNamedLockTypes { get; set; }

    bool RegisterDenormalizeHierarchicalDALListener { get; set; }

    IBssFrameworkSettings AddSecuritySystem(Action<ISecuritySystemSettings> setupAction);

    IBssFrameworkSettings AddNamedLockType(Type namedLockType);

    IBssFrameworkSettings AddListener<TListener>(bool registerSelf = false)
        where TListener : class, IDALListener;

    IBssFrameworkSettings AddExtensions(IBssFrameworkExtension extension);

    IBssFrameworkSettings SetNotificationPrincipalExtractor<T>()
        where T : INotificationPrincipalExtractor;

    IBssFrameworkSettings SetDomainObjectEventMetadata<T>()
        where T : IDomainObjectEventMetadata;

    IBssFrameworkSettings SetUserSource<TUserDomainObject>(
        Expression<Func<TUserDomainObject, Guid>> idPath,
        Expression<Func<TUserDomainObject, string>> namePath,
        Expression<Func<TUserDomainObject, bool>> filter);

    IBssFrameworkSettings SetSecurityAdministratorRule(DomainSecurityRule.RoleBaseSecurityRule rule);

    IBssFrameworkSettings SetSpecificationEvaluator<TSpecificationEvaluator>()
        where TSpecificationEvaluator : class, ISpecificationEvaluator;

    IBssFrameworkSettings AddDatabaseVisitors<TExpressionVisitorContainerItem>(bool scoped = false)
        where TExpressionVisitorContainerItem : class, IExpressionVisitorContainerItem;

    IBssFrameworkSettings AddDatabaseSettings(Action<INHibernateSetupObject> setupAction);
}
