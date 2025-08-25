using Framework.DomainDriven;
using SecuritySystem.Credential;

using Microsoft.Extensions.DependencyInjection;

namespace Automation.ServiceEnvironment;

public class RootAuthManager(IServiceProvider rootServiceProvider)
{
    protected IServiceEvaluator<AuthManager> ManagerEvaluator => rootServiceProvider.GetRequiredService<IServiceEvaluator<AuthManager>>();

    public string GetCurrentUserLogin()
    {
        return this.ManagerEvaluator.Evaluate(DBSessionMode.Read, manager => manager.GetCurrentUserLogin());
    }

    public Guid CreatePrincipal(string name)
    {
        return this.CreatePrincipalAsync(name).GetAwaiter().GetResult();
    }

    public async Task<Guid> CreatePrincipalAsync(string name, CancellationToken cancellationToken = default)
    {
        return await this.ManagerEvaluator.EvaluateAsync(DBSessionMode.Write, async manger => await manger.CreatePrincipalAsync(name, cancellationToken));
    }

    public RootUserCredentialManager For(UserCredential? userCredential = null)
    {
        return new RootUserCredentialManager(rootServiceProvider, userCredential);
    }
}
