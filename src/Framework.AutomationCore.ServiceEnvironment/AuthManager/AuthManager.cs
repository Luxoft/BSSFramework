using Automation.Utils;

using Framework.Core.Services;
using Framework.SecuritySystem.Credential;
using Framework.SecuritySystem.ExternalSystem.Management;

namespace Automation.ServiceEnvironment;

public class AuthManager(
    IUserAuthenticationService userAuthenticationService,
    IPrincipalManagementService principalManagementService,
    IUserCredentialNameResolver credentialNameResolver)
{
    public string GetCurrentUserLogin()
    {
        return userAuthenticationService.GetUserName();
    }

    public async Task<Guid> CreatePrincipalAsync(string name, CancellationToken cancellationToken = default)
    {
        var principal = await principalManagementService.CreatePrincipalAsync(name, cancellationToken);

        return principal.Id;
    }

    public async Task<Guid> AddUserRoleAsync(
        UserCredential? userCredential,
        TestPermission[] testPermissions,
        CancellationToken cancellationToken = default)
    {
        var actualUserCredential = userCredential ?? this.GetCurrentUserLogin();

        var existsPrincipal = await principalManagementService.TryGetPrincipalAsync(actualUserCredential, cancellationToken);

        var preUpdatePrincipal = existsPrincipal
                                 ?? await this.RawCreatePrincipalAsync(
                                     credentialNameResolver.GetUserName(actualUserCredential),
                                     cancellationToken);

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

        return updatedPrincipal.Header.Id;
    }

    private async Task<TypedPrincipal> RawCreatePrincipalAsync(string usedPrincipalName, CancellationToken cancellationToken)
    {
        return new TypedPrincipal(
            new TypedPrincipalHeader(
                (await principalManagementService.CreatePrincipalAsync(usedPrincipalName, cancellationToken)).Id,
                usedPrincipalName,
                false),
            []);
    }

    public async Task RemovePermissionsAsync(UserCredential? userCredential, CancellationToken cancellationToken = default)
    {
        var principal = await principalManagementService.TryGetPrincipalAsync(
                            userCredential ?? this.GetCurrentUserLogin(),
                            cancellationToken);

        if (principal is { Header.IsVirtual: false })
        {
            await principalManagementService.RemovePrincipalAsync(principal.Header.Id, true, cancellationToken);
        }
    }
}
