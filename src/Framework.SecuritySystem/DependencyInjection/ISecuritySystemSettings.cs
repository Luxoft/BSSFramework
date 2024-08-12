using Framework.Persistent;
using Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

namespace Framework.SecuritySystem.DependencyInjection;

public interface ISecuritySystemSettings
{
    bool InitializeAdministratorRole { get; set; }

    ISecuritySystemSettings AddSecurityContext<TSecurityContext>(
        Guid ident,
        string? name = null,
        Func<TSecurityContext, string>? displayFunc = null)
        where TSecurityContext : ISecurityContext, IIdentityObject<Guid>;

    ISecuritySystemSettings AddDomainSecurityServices(Action<IDomainSecurityServiceRootBuilder> setup);

    ISecuritySystemSettings AddSecurityRole(SecurityRole securityRole, SecurityRoleInfo info);

    ISecuritySystemSettings AddSecurityRule(DomainSecurityRule.SecurityRuleHeader header, DomainSecurityRule implementation);

    ISecuritySystemSettings AddSecurityOperation(SecurityOperation securityOperation, SecurityOperationInfo info);

    ISecuritySystemSettings AddExtensions(ISecuritySystemExtension extensions);

    ISecuritySystemSettings SetCurrentUserSecurityProvider(Type genericSecurityProviderType);

    ISecuritySystemSettings SetAccessDeniedExceptionService<TAccessDeniedExceptionService>()
        where TAccessDeniedExceptionService : class, IAccessDeniedExceptionService;
}
