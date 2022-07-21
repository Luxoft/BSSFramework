using System;
using System.Threading.Tasks;

using Framework.Core.Services;
using Framework.DomainDriven.ServiceModel.IAD;

using SampleSystem.WebApiCore.Env;

namespace SampleSystem.WebApiCore;

public class SampleSystemUserAuthenticationService : IUserAuthenticationService, IImpersonateService
{
    private readonly IDefaultUserAuthenticationService defaultAuthenticationService;

    public SampleSystemUserAuthenticationService(IDefaultUserAuthenticationService defaultAuthenticationService)
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
