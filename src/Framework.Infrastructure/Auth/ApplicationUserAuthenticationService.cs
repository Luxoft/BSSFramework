using CommonFramework;

using Framework.Core.Auth;

using SecuritySystem.Services;

namespace Framework.Infrastructure.Auth;

public class ApplicationUserAuthenticationService(
    IDefaultUserAuthenticationService defaultAuthenticationService,
    IUserCredentialNameResolver userCredentialNameResolver,
    IDefaultCancellationTokenSource? defaultCancellationTokenSource = null)
    : ImpersonateUserAuthenticationService(userCredentialNameResolver, defaultCancellationTokenSource)
{
    protected override string GetPureUserName() => defaultAuthenticationService.GetUserName();
}
