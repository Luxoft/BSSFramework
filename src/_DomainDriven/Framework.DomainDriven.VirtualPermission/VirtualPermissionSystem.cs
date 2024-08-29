using System.Linq.Expressions;

using Framework.SecuritySystem;
using Framework.SecuritySystem.DependencyInjection;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.DomainDriven.VirtualPermission;

public class VirtualPermissionSystem<TDomainObject>() : IPermissionSystem
{
    public bool HasAccess(DomainSecurityRule.RoleBaseSecurityRule securityRule) => throw new NotImplementedException();

    public IPermissionSource GetPermissionSource(DomainSecurityRule.RoleBaseSecurityRule securityRule) => throw new NotImplementedException();

    public Task<IEnumerable<SecurityRole>> GetAvailableSecurityRoles(CancellationToken cancellationToken = default) => throw new NotImplementedException();
}

public static class SecuritySystemSettingsExtensions
{
    public static ISecuritySystemSettings AddVirtualPermission<TDomainObject>(
        this ISecuritySystemSettings securitySystemSettings,
        VirtualPermissionBindingInfo<TDomainObject> bindingInfo,
        Expression<Func<TDomainObject, bool>>? filter = null)
    {
        throw new NotImplementedException();
    }
    public static ISecuritySystemSettings AddVirtualPermission<TDomainObject>(
        this ISecuritySystemSettings securitySystemSettings,
        SecurityRole securityRole,
        Expression<Func<TDomainObject, string>> principalNamePath,
        Func<VirtualPermissionBindingInfo<TDomainObject>, VirtualPermissionBindingInfo<TDomainObject>> initFunc,
        Expression<Func<TDomainObject, bool>>? filter = null)
    {
        return securitySystemSettings.AddVirtualPermission(
            initFunc(new VirtualPermissionBindingInfo<TDomainObject>(securityRole, principalNamePath)), filter);
    }
}
