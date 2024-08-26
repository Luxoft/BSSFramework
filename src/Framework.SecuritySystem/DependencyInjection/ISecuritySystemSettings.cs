using System.Linq.Expressions;
using Framework.Persistent;
using Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;
using Framework.SecuritySystem.UserSource;

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

    ISecuritySystemSettings SetAccessDeniedExceptionService<TAccessDeniedExceptionService>()
        where TAccessDeniedExceptionService : class, IAccessDeniedExceptionService;

    ISecuritySystemSettings SetCurrentUser<TCurrentUser>()
        where TCurrentUser : ICurrentUser;

    ISecuritySystemSettings SetUserSource<TUserDomainObject>(
        Expression<Func<TUserDomainObject, Guid>> idPath,
        Expression<Func<TUserDomainObject, string>> namePath,
        Expression<Func<TUserDomainObject, bool>> filter);

    ISecuritySystemSettings SetCurrentUserSecurityProvider(Type genericSecurityProviderType);
}
