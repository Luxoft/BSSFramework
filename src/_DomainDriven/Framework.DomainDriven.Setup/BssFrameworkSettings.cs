using System.Linq.Expressions;

using Framework.Authorization.Notification;

using Microsoft.Extensions.DependencyInjection;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.Events;
using Framework.SecuritySystem.DependencyInjection;
using Framework.Authorization.SecuritySystem;
using Framework.DomainDriven._Visitors;
using Framework.DomainDriven.NHibernate;
using Framework.SecuritySystem;

using nuSpec.Abstraction;
using Framework.Authorization.SecuritySystem.UserSource;
using Framework.DependencyInjection;

namespace Framework.DomainDriven.Setup;

public class BssFrameworkSettings : IBssFrameworkSettings
{
    private readonly List<Action<ISecuritySystemSettings>> additionalSecuritySystemSettingsActions = new();

    public List<Type> NamedLockTypes { get; set; } = new();

    public bool RegisterBaseNamedLockTypes { get; set; } = true;

    public bool RegisterDenormalizeHierarchicalDALListener { get; set; } = true;

    public List<Action<IServiceCollection>> RegisterActions { get; set; } = new();

    public List<IBssFrameworkExtension> Extensions = new();

    public Type NotificationPrincipalExtractorType { get; private set; }

    public Type DomainObjectEventMetadataType { get; private set; }

    public DomainSecurityRule.RoleBaseSecurityRule SecurityAdministratorRule { get; private set; } = SecurityRole.Administrator;

    public IBssFrameworkSettings AddSecuritySystem(Action<ISecuritySystemSettings> setupAction)
    {
        this.RegisterActions.Add(
            sc => sc.AddSecuritySystem(
                sss =>
                {
                    setupAction(sss);
                    this.additionalSecuritySystemSettingsActions.ForEach(a => a(sss));
                }));

        return this;
    }

    public IBssFrameworkSettings AddNamedLockType(Type namedLockType)
    {
        this.NamedLockTypes.Add(namedLockType);

        return this;
    }

    public IBssFrameworkSettings AddListener<TListener>(bool registerSelf = false)
        where TListener : class, IDALListener
    {
        this.RegisterActions.Add(sc => sc.RegisterListeners(s => s.Add<TListener>(registerSelf)));

        return this;
    }

    public IBssFrameworkSettings AddExtensions(IBssFrameworkExtension extension)
    {
        this.Extensions.Add(extension);

        return this;
    }

    public IBssFrameworkSettings SetNotificationPrincipalExtractor<T>()
        where T : INotificationPrincipalExtractor
    {
        this.NotificationPrincipalExtractorType = typeof(T);

        return this;
    }

    public IBssFrameworkSettings SetDomainObjectEventMetadata<T>()
        where T : IDomainObjectEventMetadata
    {
        this.DomainObjectEventMetadataType = typeof(T);

        return this;
    }

    public IBssFrameworkSettings SetUserSource<TUserDomainObject>(
        Expression<Func<TUserDomainObject, bool>> filter,
        Expression<Func<TUserDomainObject, string>> namePath,
        Expression<Func<TUserDomainObject, Guid>> idPath)
    {
        this.RegisterActions.Add(
            sc =>
            {
                sc.AddSingleton(new UserPathInfo<TUserDomainObject>(filter, namePath, idPath));

                sc.AddScoped<ICurrentUserSource<TUserDomainObject>, CurrentUserSource<TUserDomainObject>>();
                sc.AddScopedFrom<ICurrentUserSource, ICurrentUserSource<TUserDomainObject>>();

                sc.AddScoped<IUserPathInfoRelativeService, UserPathInfoRelativeService<TUserDomainObject>>();
                sc.AddScoped(typeof(CurrentUserSecurityProvider<>));

                sc.AddScoped<IPrincipalIdentitySource, PrincipalIdentitySource<TUserDomainObject>>();
            });

        this.additionalSecuritySystemSettingsActions.Add(
            securitySystemBuilder => { securitySystemBuilder.SetCurrentUserSecurityProvider(typeof(CurrentUserSecurityProvider<>)); });

        return this;
    }

    public IBssFrameworkSettings SetSecurityAdministratorRule(DomainSecurityRule.RoleBaseSecurityRule rule)
    {
        this.SecurityAdministratorRule = rule;

        return this;
    }

    public IBssFrameworkSettings SetSpecificationEvaluator<TSpecificationEvaluator>()
        where TSpecificationEvaluator : class, ISpecificationEvaluator
    {
        this.RegisterActions.Add(sc => sc.AddSingleton<ISpecificationEvaluator, TSpecificationEvaluator>());

        return this;
    }

    public IBssFrameworkSettings AddDatabaseVisitors<TExpressionVisitorContainerItem>(bool scoped = false)
        where TExpressionVisitorContainerItem : class, IExpressionVisitorContainerItem
    {
        this.RegisterActions.Add(
            sc =>
            {
                if (scoped)
                {
                    sc.AddScoped<IExpressionVisitorContainerItem, TExpressionVisitorContainerItem>();
                }
                else
                {
                    sc.AddSingleton<IExpressionVisitorContainerItem, TExpressionVisitorContainerItem>();
                }
            });

        return this;
    }

    public IBssFrameworkSettings AddDatabaseSettings(Action<INHibernateSetupObject> setup)
    {
        this.RegisterActions.Add(sc => sc.AddDatabaseSettings(setup));

        return this;
    }
}
