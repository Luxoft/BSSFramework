using System;
using System.Threading.Tasks;

using Framework.Core.Services;

namespace Framework.DomainDriven.WebApiNetCore;

public class ApplicationUserAuthenticationService : IUserAuthenticationService, IImpersonateService
{
    private readonly IDefaultUserAuthenticationService defaultAuthenticationService;

    public ApplicationUserAuthenticationService(IDefaultUserAuthenticationService defaultAuthenticationService)
    {
        this.defaultAuthenticationService = defaultAuthenticationService;
    }

    public string GetUserName() => this.CustomUserName ?? this.defaultAuthenticationService.GetUserName();

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
