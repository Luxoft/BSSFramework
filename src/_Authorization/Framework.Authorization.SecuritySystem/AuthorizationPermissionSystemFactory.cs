using SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

using SecuritySystem.ExternalSystem;

namespace Framework.Authorization.SecuritySystemImpl;

public class AuthorizationPermissionSystemFactory(IServiceProvider serviceProvider) : IPermissionSystemFactory
{
    public IPermissionSystem Create(SecurityRuleCredential securityRuleCredential) =>
        ActivatorUtilities.CreateInstance<AuthorizationPermissionSystem>(serviceProvider, securityRuleCredential);
}
