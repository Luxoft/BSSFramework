using Framework.Core.Services;

namespace Framework.DomainDriven.Auth;

public class ApplicationUserAuthenticationService(IDefaultUserAuthenticationService defaultAuthenticationService)
    : IUserAuthenticationService, IImpersonateService
{
    public string GetUserName() => this.CustomUserName ?? defaultAuthenticationService.GetUserName();

    public string CustomUserName { get; private set; }

    public async Task<T> WithImpersonateAsync<T>(string customUserName, Func<Task<T>> func)
    {
        var prev = this.CustomUserName;

        this.CustomUserName = customUserName;

        try
        {
            return await func();
        }
        finally
        {
            this.CustomUserName = prev;
        }
    }
}
