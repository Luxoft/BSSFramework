using System;
using System.Threading.Tasks;

using SampleSystem.WebApiCore.Env;

namespace SampleSystem.IntegrationTests.__Support.ServiceEnvironment;

public class IntegrationTestDefaultUserAuthenticationService : DomainDefaultUserAuthenticationService
{
    public string CustomUserName { get; private set; }

    public override string GetUserName()
    {
        return this.CustomUserName ?? base.GetUserName();
    }

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
