using Automation.ServiceEnvironment.Services;
using Automation.Utils;

using Framework.DomainDriven;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Credential;
using Framework.SecuritySystem.ExternalSystem.Management;
using Microsoft.Extensions.DependencyInjection;

namespace Automation.ServiceEnvironment;

public class RootUserCredentialManager(IServiceProvider rootServiceProvider, UserCredential? userCredential)
{
    private IServiceEvaluator<AuthManager> ManagerEvaluator => rootServiceProvider.GetRequiredService<IServiceEvaluator<AuthManager>>();

    private IIntegrationTestUserAuthenticationService UserAuthenticationService =>
        rootServiceProvider.GetRequiredService<IIntegrationTestUserAuthenticationService>();

    private IEnumerable<SecurityRole> AdminRoles => rootServiceProvider.GetRequiredService<AdministratorsRoleList>().Roles;

    public void LoginAs()
    {
        this.UserAuthenticationService.SetUser(userCredential);
    }

    public Guid SetAdminRole()
    {
        return this.SetAdminRoleAsync().GetAwaiter().GetResult();
    }

    public Task<Guid> SetAdminRoleAsync(CancellationToken cancellationToken = default)
    {
        return this.SetRoleAsync(this.AdminRoles.Select(v => (TestPermission)v).ToArray(), cancellationToken);
    }

    public Guid SetRole(params TestPermission[] permissions)
    {
        return this.SetRoleAsync(permissions).GetAwaiter().GetResult();
    }

    public async Task<Guid> SetRoleAsync(TestPermission[] permissions, CancellationToken cancellationToken = default)
    {
        await this.ClearRolesAsync(cancellationToken);

        return await this.AddRoleAsync(permissions, cancellationToken);
    }

    public async Task<Guid> SetRoleAsync(TestPermission permission, CancellationToken cancellationToken = default)
    {
        return await this.AddRoleAsync([permission], cancellationToken);
    }

    public Guid AddRole(params TestPermission[] permissions) =>
        this.AddRoleAsync(permissions).GetAwaiter().GetResult();

    public async Task<Guid> AddRoleAsync(TestPermission[] permissions, CancellationToken cancellationToken = default) =>
        await this.ManagerEvaluator.EvaluateAsync(
            DBSessionMode.Write,
            async manger => await manger.AddUserRoleAsync(userCredential, permissions, cancellationToken));

    public async Task<Guid> AddRoleAsync(TestPermission permission, CancellationToken cancellationToken = default) =>
        await this.AddRoleAsync([permission], cancellationToken);

    public void ClearRoles()
    {
        this.ClearRolesAsync().GetAwaiter().GetResult();
    }

    public async Task ClearRolesAsync(CancellationToken cancellationToken = default)
    {
        await this.ManagerEvaluator.EvaluateAsync(
            DBSessionMode.Write,
            async manager => await manager.RemovePermissionsAsync(userCredential, cancellationToken));
    }

    public TypedPrincipal GetPrincipal()
    {
        return this.GetPrincipalAsync().GetAwaiter().GetResult();
    }

    public async Task<TypedPrincipal> GetPrincipalAsync(CancellationToken cancellationToken = default)
    {
        return await this.ManagerEvaluator.EvaluateAsync(
                   DBSessionMode.Write,
                   async manager => await manager.GetPrincipalAsync(userCredential, cancellationToken));
    }
}
