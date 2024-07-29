using System.Linq.Expressions;

using Framework.Authorization.Notification;

using Microsoft.Extensions.DependencyInjection;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.Events;
using Framework.SecuritySystem.DependencyInjection;
using Framework.Authorization.SecuritySystem;
using Framework.Persistent;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.Setup;

public class BssFrameworkSettings : IBssFrameworkSettings
{
    public List<Type> NamedLockTypes { get; set; } = new();

    public bool RegisterBaseNamedLockTypes { get; set; } = true;

    public bool RegisterDenormalizeHierarchicalDALListener { get; set; } = true;

    public List<Action<IServiceCollection>> RegisterActions { get; set; } = new();

    public List<IBssFrameworkExtension> Extensions = new();

    public Type NotificationPrincipalExtractorType { get; private set; }

    public Type DomainObjectEventMetadataType { get; private set; }

    public DomainSecurityRule.RoleBaseSecurityRule SecurityAdministratorRule { get; private set; } = SecurityRole.Administrator;

    public IBssFrameworkSettings AddSecuritySystem(Action<ISecuritySystemSettings> settings)
    {
        this.RegisterActions.Add(sc => sc.AddSecuritySystem(settings));

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

    public IBssFrameworkSettings SetPrincipalIdentitySource<TDomainObject>(Expression<Func<TDomainObject, bool>> filter, Expression<Func<TDomainObject, string>> namePath)
        where TDomainObject : IIdentityObject<Guid>
    {
        this.RegisterActions.Add(sc => sc.AddScoped<IPrincipalIdentitySource, PrincipalIdentitySource<TDomainObject>>());
        this.RegisterActions.Add(sc => sc.AddSingleton(new PrincipalIdentitySourcePathInfo<TDomainObject>(filter, namePath)));

        return this;
    }

    public IBssFrameworkSettings SetSecurityAdministratorRule(DomainSecurityRule.RoleBaseSecurityRule rule)
    {
        this.SecurityAdministratorRule = rule;

        return this;
    }
}
