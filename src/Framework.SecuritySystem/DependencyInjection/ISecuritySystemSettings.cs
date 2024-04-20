using Framework.Persistent;
using Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

namespace Framework.SecuritySystem.DependencyInjection;

public interface ISecuritySystemSettings
{
    bool InitializeAdministratorRole { get; set; }

    ISecuritySystemSettings AddSecurityContext<TSecurityContext>(
        Guid ident,
        string name = null,
        Func<TSecurityContext, string> displayFunc = null)
        where TSecurityContext : ISecurityContext, IIdentityObject<Guid>;

    ISecuritySystemSettings AddDomainSecurityServices(Action<IDomainSecurityServiceRootBuilder> setup);

    ISecuritySystemSettings AddSecurityRole(SecurityRole securityRole, SecurityRoleInfo info);
}
