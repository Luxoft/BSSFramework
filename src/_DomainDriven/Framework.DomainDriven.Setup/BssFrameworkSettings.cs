using Framework.Persistent;
using Framework.SecuritySystem;
using Framework.SecuritySystem.DependencyInjection;
using Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.Setup;

public class BssFrameworkSettings : IBssFrameworkSettings
{
    public List<Type> SecurityOperationTypes { get; set; } = new();

    public List<Type> SecurityRoleTypes { get; set; } = new();

    public List<Type> NamedLockTypes { get; set; } = new();

    public bool RegisterBaseSecurityOperationTypes { get; set; } = true;

    public bool RegisterBaseNamedLockTypes { get; set; } = true;

    public List<Action<IServiceCollection>> RegisterSecurityContextActions { get; set; } = new();

    public List<Action<IServiceCollection>> RegisterDomainSecurityServicesActions { get; set; } = new();

    public List<IBssFrameworkExtension> Extensions = new();


    public IBssFrameworkSettings AddSecurityOperationType(Type securityOperationType)
    {
        this.SecurityOperationTypes.Add(securityOperationType);

        return this;
    }

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
        this.RegisterSecurityContextActions.Add(sc => sc.RegisterSecurityContextInfoService(setup));

        return this;
    }

    public IBssFrameworkSettings AddDomainSecurityServices(Action<IDomainSecurityServiceRootBuilder> setup)
    {
        this.RegisterDomainSecurityServicesActions.Add(sc => sc.RegisterDomainSecurityServices<Guid>(setup));

        return this;
    }

    public IBssFrameworkSettings AddExtensions(IBssFrameworkExtension extension)
    {
        this.Extensions.Add(extension);

        return this;
    }
}
