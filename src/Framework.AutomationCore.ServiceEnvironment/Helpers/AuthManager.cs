using Automation.Utils;

using Framework.Core.Services;
using Framework.SecuritySystem.ExternalSystem.Management;

namespace Automation.ServiceEnvironment;

public class AuthManager(
    IUserAuthenticationService userAuthenticationService,
    IPrincipalManagementService principalManagementService)
{
    public string GetCurrentUserLogin()
    {
        return userAuthenticationService.GetUserName();
    }

    public async Task<Guid> SavePrincipalAsync(string name, CancellationToken cancellationToken = default)
    {
        var principal = await principalManagementService.CreatePrincipalAsync(name, cancellationToken);

        return principal.Id;
    }

    public async Task AddUserRoleAsync(
        string? principalName,
        TestPermission[] testPermissions,
        CancellationToken cancellationToken = default)
    {
        var usedPrincipalName = principalName ?? this.GetCurrentUserLogin();

        var existsPrincipal = await principalManagementService.TryGetPrincipalAsync(usedPrincipalName, cancellationToken);

        var preUpdatePrincipal = existsPrincipal
                                 ?? new TypedPrincipal(
                                     new TypedPrincipalHeader(
                                         (await principalManagementService.CreatePrincipalAsync(usedPrincipalName, cancellationToken)).Id,
                                         usedPrincipalName,
                                         false),
                                     []);

        var newPermissions = testPermissions.Select(
            testPermission => new TypedPermission(
                Guid.Empty,
                false,
                testPermission.SecurityRole,
                testPermission.Period,
                "",
                testPermission.Restrictions));

        var updatedPrincipal = preUpdatePrincipal with { Permissions = preUpdatePrincipal.Permissions.Concat(newPermissions).ToList() };

        await principalManagementService.UpdatePermissionsAsync(
            updatedPrincipal.Header.Id,
            updatedPrincipal.Permissions,
            cancellationToken);
    }

    public async Task RemovePermissionsAsync(string? principalName, CancellationToken cancellationToken = default)
    {
        var principal = await principalManagementService.TryGetPrincipalAsync(
                            principalName ?? this.GetCurrentUserLogin(),
                            cancellationToken);

        if (principal is { Header.IsVirtual: false })
        {
            await principalManagementService.RemovePrincipalAsync(principal.Header.Id, true, cancellationToken);
        }
    }
}
