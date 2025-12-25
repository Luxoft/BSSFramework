using SecuritySystem.Services;

namespace Framework.DomainDriven.Auth;

public class ApplicationUserAuthenticationService(IDefaultUserAuthenticationService defaultAuthenticationService, IUserCredentialNameResolver userCredentialNameResolver)
    : ImpersonateUserAuthenticationService(userCredentialNameResolver)
{
    protected override string GetPureUserName() => defaultAuthenticationService.GetUserName();
}
