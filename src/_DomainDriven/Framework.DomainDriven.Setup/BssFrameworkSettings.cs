using Framework.Authorization.Notification;
using Framework.Persistent;
using Framework.SecuritySystem;
using Framework.SecuritySystem.DependencyInjection;
using Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

using Microsoft.Extensions.DependencyInjection;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.Events;

namespace Framework.DomainDriven.Setup;

public class BssFrameworkSettings : IBssFrameworkSettings
{
    public List<Type> SecurityRoleTypes { get; set; } = new();

    public List<Type> NamedLockTypes { get; set; } = new();

    public bool RegisterBaseNamedLockTypes { get; set; } = true;

    public bool RegisterDenormalizeHierarchicalDALListener { get; set; } = true;

    public List<Action<IServiceCollection>> RegisterActions { get; set; } = new();

    public List<IBssFrameworkExtension> Extensions = new();

    public Type NotificationPrincipalExtractorType { get; private set; }

    public Type DomainObjectEventMetadataType { get; private set; }


    public SecurityRole AdministratorRole { get; private set; }

    public SecurityRole SystemIntegrationRole { get; private set; }

    public IBssFrameworkSettings AddSecurityRoleTypeType(Type securityRoleType)
    {
        this.SecurityRoleTypes.Add(securityRoleType);

        return this;
    }

    public IBssFrameworkSettings AddNamedLockType(Type namedLockType)
    {
        this.NamedLockTypes.Add(namedLockType);

        return this;
    }


    public IBssFrameworkSettings AddSecurityContext<TSecurityContext>(Guid ident, string name = null, Func<TSecurityContext, string> displayFunc = null)
        where TSecurityContext : ISecurityContext, IIdentityObject<Guid>
    {
        return this.AddSecurityContext(b => b.Add(ident, name, displayFunc));
    }

    public IBssFrameworkSettings AddSecurityContext(Action<ISecurityContextInfoBuilder<Guid>> setup)
    {
        this.RegisterActions.Add(sc => sc.RegisterSecurityContextInfoService(setup));

        return this;
    }

    public IBssFrameworkSettings AddDomainSecurityServices(Action<IDomainSecurityServiceRootBuilder> setup)
    {
        this.RegisterActions.Add(sc => sc.RegisterDomainSecurityServices<Guid>(setup));

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
}
