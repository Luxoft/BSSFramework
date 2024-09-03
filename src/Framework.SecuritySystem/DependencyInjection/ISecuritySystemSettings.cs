using System.Linq.Expressions;

using Framework.Persistent;
using Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;
using Framework.SecuritySystem.ExternalSystem;
using Framework.SecuritySystem.SecurityAccessor;
using Framework.SecuritySystem.Services;
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

    ISecuritySystemSettings AddPermissionSystem<TPermissionSystem>()
        where TPermissionSystem : class, IPermissionSystem;

    ISecuritySystemSettings AddPermissionSystem<TPermissionSystem>(Func<IServiceProvider, TPermissionSystem> factory)
        where TPermissionSystem : class, IPermissionSystem;

    ISecuritySystemSettings AddExtensions(ISecuritySystemExtension extensions);

    ISecuritySystemSettings SetAccessDeniedExceptionService<TAccessDeniedExceptionService>()
        where TAccessDeniedExceptionService : class, IAccessDeniedExceptionService;

    ISecuritySystemSettings SetRunAsManager<TRunAsManager>()
        where TRunAsManager : class, IRunAsManager;

    ISecuritySystemSettings SetUserSource<TUserDomainObject>(
        Expression<Func<TUserDomainObject, Guid>> idPath,
        Expression<Func<TUserDomainObject, string>> namePath,
        Expression<Func<TUserDomainObject, bool>> filter);


    ISecuritySystemSettings SetSecurityAccessorInfinityStorage<TStorage>()
        where TStorage : class, ISecurityAccessorInfinityStorage;

}
