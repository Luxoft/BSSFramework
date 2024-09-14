using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.SecuritySystem;

public class AuthorizationPermissionSystemFactory(
    IServiceProvider serviceProvider) : IPermissionSystemFactory
{
    public IPermissionSystem Create(SecurityRuleCredential securityRuleCredential) =>
        ActivatorUtilities.CreateInstance<AuthorizationPermissionSystem>(serviceProvider, securityRuleCredential);
}
