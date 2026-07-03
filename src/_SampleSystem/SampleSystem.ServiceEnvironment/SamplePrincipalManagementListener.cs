using Anch.SecuritySystem.ExternalSystem.Management;

using Framework.Authorization.Domain;

using Microsoft.Extensions.Logging;

namespace SampleSystem.ServiceEnvironment;

public class SamplePrincipalManagementListener(ILogger<SamplePrincipalManagementListener> logger)
    : IPrincipalManagementListener<Principal, Permission, PermissionRestriction>
{
    public Task PrincipalCreatedAsync(PrincipalData<Principal, Permission, PermissionRestriction> principal, CancellationToken ct)
    {
        logger.LogInformation("Principal with {Id} has been created", principal.Principal.Id);

        return Task.CompletedTask;
    }

    public Task PrincipalChangedAsync(PrincipalData<Principal, Permission, PermissionRestriction> principal, CancellationToken ct) => Task.CompletedTask;

    public Task PrincipalRemovedAsync(PrincipalData<Principal, Permission, PermissionRestriction> principal, CancellationToken ct) => Task.CompletedTask;

    public Task PermissionCreatedAsync(PermissionData<Permission, PermissionRestriction> permission, CancellationToken ct) => Task.CompletedTask;

    public Task PermissionChangedAsync(PermissionData<Permission, PermissionRestriction> permission, CancellationToken ct) => Task.CompletedTask;

    public Task PermissionRemovedAsync(PermissionData<Permission, PermissionRestriction> permission, CancellationToken ct) => Task.CompletedTask;
}
