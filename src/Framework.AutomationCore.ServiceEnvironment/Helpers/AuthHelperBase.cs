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

    protected TResult EvaluateManager<TResult>(Func<AuthManager, TResult> getResult)
    {
        return this.RootServiceProvider.GetRequiredService<IServiceEvaluator<AuthManager>>().Evaluate(DBSessionMode.Write, getResult);
    }

    protected void EvaluateManager(Action<AuthManager> action)
    {
        this.EvaluateManager<object>(
            manager =>
            {
                action(manager);
                return default;
            });
    }

    public string GetCurrentUserLogin()
    {
        return this.EvaluateManager(manager => manager.GetCurrentUserLogin());
    }

    public void LoginAs(string principalName = null)
    {
        this.RootServiceProvider.GetRequiredService<IIntegrationTestUserAuthenticationService>().SetUserName(principalName);
    }

    public Guid SavePrincipal(string name, bool active, Guid? externalId = null)
    {
        return this.EvaluateManager(manger => manger.SavePrincipal(name, active, externalId));
    }

    public void AddUserRole(string principalName, params TestPermission[] permissions)
    {
        this.EvaluateManager(manger => manger.AddUserRole(principalName, permissions));
    }

    public void SetCurrentUserRole(params TestPermission[] permissions)
    {
        this.SetUserRole(default, permissions);
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
        this.SetCurrentUserRole(SecurityRole.Administrator, SecurityRole.SystemIntegration);
    }

    private void RemovePermissions(string principalName)
    {
        this.EvaluateManager(manager => manager.RemovePermissions(principalName));
    }
}
