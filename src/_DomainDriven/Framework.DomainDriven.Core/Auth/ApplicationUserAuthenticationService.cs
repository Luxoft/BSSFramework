using Framework.Core.Services;
using Framework.SecuritySystem.Credential;

namespace Framework.DomainDriven.Auth;

public class ApplicationUserAuthenticationService(IDefaultUserAuthenticationService defaultAuthenticationService, IUserCredentialNameResolver userCredentialNameResolver)
    : IUserAuthenticationService, IImpersonateService
{
    public string GetUserName() => this.CustomUserCredential == null
                                       ? defaultAuthenticationService.GetUserName()
                                       : userCredentialNameResolver.GetUserName(this.CustomUserCredential);

    public UserCredential? CustomUserCredential { get; private set; }

    public async Task<T> WithImpersonateAsync<T>(UserCredential? customUserCredential, Func<Task<T>> func)
    {
        var prev = this.CustomUserCredential;

        this.CustomUserCredential = customUserCredential;

        try
        {
            return await func();
        }
        finally
        {
            this.CustomUserCredential = prev;
        }
    }
}
