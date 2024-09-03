using System.Linq.Expressions;

using Framework.SecuritySystem;
using Framework.SecuritySystem.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.VirtualPermission;

public static class SecuritySystemSettingsExtensions
{
    public static ISecuritySystemSettings AddVirtualPermission<TDomainObject>(
        this ISecuritySystemSettings securitySystemSettings,
        Func<IServiceProvider, VirtualPermissionBindingInfo<TDomainObject>> getBindingInfo) =>
        securitySystemSettings.AddPermissionSystem(
            sp => ActivatorUtilities.CreateInstance<VirtualPermissionSystem<TDomainObject>>(sp, getBindingInfo(sp)));

    public static ISecuritySystemSettings AddVirtualPermission<TDomainObject>(
        this ISecuritySystemSettings securitySystemSettings,
        SecurityRole securityRole,
        Expression<Func<TDomainObject, string>> principalNamePath,
        Func<VirtualPermissionBindingInfo<TDomainObject>, VirtualPermissionBindingInfo<TDomainObject>>? initFunc = null)
    {
        var bindingInfo =
            (initFunc ?? (v => v)).Invoke(new VirtualPermissionBindingInfo<TDomainObject>(securityRole, principalNamePath));

        return securitySystemSettings.AddVirtualPermission(_ => bindingInfo);
    }
}
