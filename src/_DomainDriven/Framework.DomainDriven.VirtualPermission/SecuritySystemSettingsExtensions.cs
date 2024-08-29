using System.Linq.Expressions;

using Framework.SecuritySystem;
using Framework.SecuritySystem.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.VirtualPermission;

public static class SecuritySystemSettingsExtensions
{
    public static ISecuritySystemSettings AddVirtualPermission<TDomainObject>(
        this ISecuritySystemSettings securitySystemSettings,
        VirtualPermissionBindingInfo<TDomainObject> bindingInfo)
    {
        return securitySystemSettings.AddPermissionSystem(
            sp => ActivatorUtilities.CreateInstance<VirtualPermissionSystem<TDomainObject>>(sp, bindingInfo));
    }

    public static ISecuritySystemSettings AddVirtualPermission<TDomainObject>(
        this ISecuritySystemSettings securitySystemSettings,
        SecurityRole securityRole,
        Expression<Func<TDomainObject, string>> principalNamePath,
        Func<VirtualPermissionBindingInfo<TDomainObject>, VirtualPermissionBindingInfo<TDomainObject>> initFunc)
    {
        return securitySystemSettings.AddVirtualPermission(
            initFunc(new VirtualPermissionBindingInfo<TDomainObject>(securityRole, principalNamePath)));
    }
}
