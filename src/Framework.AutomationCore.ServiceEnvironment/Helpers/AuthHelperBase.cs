using Automation.ServiceEnvironment.Services;
using Automation.Utils;

using Framework.DomainDriven;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Credential;

using Microsoft.Extensions.DependencyInjection;

namespace Automation.ServiceEnvironment;

public class AuthHelperBase(IServiceProvider rootServiceProvider) : RootServiceProviderContainer(rootServiceProvider)
{
    protected IServiceEvaluator<AuthManager> ManagerEvaluator => this.RootServiceProvider.GetRequiredService<IServiceEvaluator<AuthManager>>();

    protected IIntegrationTestUserAuthenticationService UserAuthenticationService => this.RootServiceProvider.GetRequiredService<IIntegrationTestUserAuthenticationService>();

    public string GetCurrentUserLogin()
    {
        return this.ManagerEvaluator.Evaluate(DBSessionMode.Read, manager => manager.GetCurrentUserLogin());
    }

    public void LoginAs(UserCredential? userCredential = null)
    {
        this.UserAuthenticationService.SetUser(userCredential);
    }

    public Guid CreatePrincipal(string name)
    {
        return this.CreatePrincipalAsync(name).GetAwaiter().GetResult();
    }

    public async Task<Guid> CreatePrincipalAsync(string name, CancellationToken cancellationToken = default)
    {
        return await this.ManagerEvaluator.EvaluateAsync(DBSessionMode.Write, async manger => await manger.CreatePrincipalAsync(name, cancellationToken));
    }

    public Guid AddUserRole(UserCredential? userCredential, params TestPermission[] permissions) =>
        this.AddUserRoleAsync(userCredential, permissions).GetAwaiter().GetResult();

    public async Task<Guid> AddUserRoleAsync(
        UserCredential? userCredential,
        TestPermission[] permissions,
        CancellationToken cancellationToken = default) =>
        await this.ManagerEvaluator.EvaluateAsync(
            DBSessionMode.Write,
            async manger => await manger.AddUserRoleAsync(userCredential, permissions, cancellationToken));

    public virtual Guid AddUserToAdmin(UserCredential? userCredential)
    {
        return this.SetUserRole(userCredential, SecurityRole.Administrator, SecurityRole.SystemIntegration);
    }

    public Guid SetUserRole(UserCredential? userCredential, params TestPermission[] permissions)
    {
        this.RemovePermissions(userCredential);

        return this.AddUserRole(userCredential, permissions);
    }

    public virtual void AddCurrentUserToAdmin()
    {
        this.AddUserToAdmin(null);
    }

    public void SetCurrentUserRole(params TestPermission[] permissions)
    {
        this.SetUserRole(default, permissions);
    }

    public void RemovePermissions(UserCredential? userCredential)
    {
        this.RemovePermissionsAsync(userCredential).GetAwaiter().GetResult();
    }

    public async Task RemovePermissionsAsync(UserCredential? userCredential, CancellationToken cancellationToken = default)
    {
        await this.ManagerEvaluator.EvaluateAsync(DBSessionMode.Write, async manager => await manager.RemovePermissionsAsync(userCredential, cancellationToken));
    }
}
