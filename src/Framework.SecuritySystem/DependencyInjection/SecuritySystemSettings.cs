using Framework.Persistent;
using Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem.DependencyInjection;

public class SecuritySystemSettings : ISecuritySystemSettings
{
    public List<Action<IServiceCollection>> RegisterActions { get; set; } = new();

    public ISecuritySystemSettings AddSecurityContext<TSecurityContext>(Guid ident, string name = null, Func<TSecurityContext, string> displayFunc = null)
        where TSecurityContext : ISecurityContext, IIdentityObject<Guid>
    {
        return this.AddSecurityContext(b => b.Add(ident, name, displayFunc));
    }

    public ISecuritySystemSettings AddSecurityContext(Action<ISecurityContextInfoBuilder<Guid>> setup)
    {
        this.RegisterActions.Add(sc => sc.RegisterSecurityContextInfoService(setup));

        return this;
    }

    public ISecuritySystemSettings AddDomainSecurityServices(Action<IDomainSecurityServiceRootBuilder> setup)
    {
        this.RegisterActions.Add(sc => sc.RegisterDomainSecurityServices<Guid>(setup));

        return this;
    }

    public ISecuritySystemSettings AddSecurityRole(SecurityRole securityRole, SecurityRoleInfo info)
    {
        if (securityRole == SecurityRole.Administrator)
        {

        }

        throw new NotImplementedException();
    }
}
