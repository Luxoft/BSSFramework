using Automation.ServiceEnvironment.Services;
using Automation.Utils;

using Framework.DomainDriven;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Automation.ServiceEnvironment;

public class AuthHelperBase : RootServiceProviderContainer
{
    public AuthHelperBase(IServiceProvider rootServiceProvider)
        : base(rootServiceProvider)
    {
    }

    protected IServiceEvaluator<AuthManager> ManagerEvaluator => this.RootServiceProvider.GetRequiredService<IServiceEvaluator<AuthManager>>();

    protected IIntegrationTestUserAuthenticationService UserAuthenticationService => this.RootServiceProvider.GetRequiredService<IIntegrationTestUserAuthenticationService>();

    public string GetCurrentUserLogin()
    {
        return this.ManagerEvaluator.Evaluate(DBSessionMode.Read, manager => manager.GetCurrentUserLogin());
    }

    public void LoginAs(string principalName = null)
    {
        this.UserAuthenticationService.SetUserName(principalName);
    }


    public Guid SavePrincipal(string name)
    {
        return this.SavePrincipalAsync(name).GetAwaiter().GetResult();
    }

    public async Task<Guid> SavePrincipalAsync(string name, CancellationToken cancellationToken = default)
    {
        return await this.ManagerEvaluator.EvaluateAsync(DBSessionMode.Write, async manger => await manger.SavePrincipalAsync(name, cancellationToken));
    }

    public void AddUserRole(string principalName, params TestPermission[] permissions)
    {
        this.AddUserRoleAsync(principalName, permissions).GetAwaiter().GetResult();
    }

    public async Task AddUserRoleAsync(string principalName, TestPermission[] permissions, CancellationToken cancellationToken = default)
    {
        await this.ManagerEvaluator.EvaluateAsync(DBSessionMode.Write, async manger => await manger.AddUserRoleAsync(principalName, permissions, cancellationToken));
    }

    public virtual void AddUserToAdmin(string principalName)
    {
        this.SetUserRole(principalName, SecurityRole.Administrator, SecurityRole.SystemIntegration);
    }

    public void SetUserRole(string principalName, params TestPermission[] permissions)
    {
        if (permissions == null)
        {
            throw new ArgumentNullException(nameof(permissions));
        }

        this.RemovePermissions(principalName);

        this.AddUserRole(principalName, permissions);
    }


    public virtual void AddCurrentUserToAdmin()
    {
        this.AddUserToAdmin(null);
    }

    public void SetCurrentUserRole(params TestPermission[] permissions)
    {
        this.SetUserRole(default, permissions);
    }

    public void RemovePermissions(string principalName)
    {
        this.RemovePermissionsAsync(principalName).GetAwaiter().GetResult();
    }

    public async Task RemovePermissionsAsync(string principalName, CancellationToken cancellationToken = default)
    {
        await this.ManagerEvaluator.EvaluateAsync(DBSessionMode.Write, async manager => await manager.RemovePermissionsAsync(principalName, cancellationToken));
    }
}
