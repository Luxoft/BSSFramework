using SecuritySystem.Services;

namespace Framework.DomainDriven.Auth;

public class ApplicationUserAuthenticationService(
    IDefaultUserAuthenticationService defaultAuthenticationService,
    IUserCredentialNameResolver userCredentialNameResolver,
    IDefaultCancellationTokenSource? defaultCancellationTokenSource = null)
    : ImpersonateUserAuthenticationService(userCredentialNameResolver, defaultCancellationTokenSource)
{
    protected override string GetPureUserName() => defaultAuthenticationService.GetUserName();
}
