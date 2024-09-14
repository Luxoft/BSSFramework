using System.Linq.Expressions;

using Framework.Persistent;
using Framework.SecuritySystem;
using Framework.SecuritySystem.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.VirtualPermission;

public static class SecuritySystemSettingsExtensions
{
    public static ISecuritySystemSettings AddVirtualPermission<TPrincipal, TPermission>(
        this ISecuritySystemSettings securitySystemSettings,
        Func<IServiceProvider, VirtualPermissionBindingInfo<TPrincipal, TPermission>> getBindingInfo)
        where TPrincipal : IIdentityObject<Guid>
        where TPermission : IIdentityObject<Guid> =>
        securitySystemSettings.AddPermissionSystem(
            sp => ActivatorUtilities.CreateInstance<VirtualPermissionSystemFactory<TPrincipal, TPermission>>(sp, getBindingInfo(sp)));

    public static ISecuritySystemSettings AddVirtualPermission<TPrincipal, TPermission>(
        this ISecuritySystemSettings securitySystemSettings,
        SecurityRole securityRole,
        Expression<Func<TPermission, TPrincipal>> principalPath,
        Expression<Func<TPrincipal, string>> principalNamePath,
        Func<VirtualPermissionBindingInfo<TPrincipal, TPermission>, VirtualPermissionBindingInfo<TPrincipal, TPermission>>? initFunc = null)
        where TPrincipal : IIdentityObject<Guid>
        where TPermission : IIdentityObject<Guid>
    {
        var bindingInfo =
            (initFunc ?? (v => v)).Invoke(
                new VirtualPermissionBindingInfo<TPrincipal, TPermission>(securityRole, principalPath, principalNamePath));

        return securitySystemSettings.AddVirtualPermission(_ => bindingInfo);
    }
}
