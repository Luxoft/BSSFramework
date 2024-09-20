using Automation.Settings;
using Microsoft.Extensions.Options;

namespace Automation.ServiceEnvironment.Services;

public class IntegrationTestUserAuthenticationService(IOptions<AutomationFrameworkSettings> settings)
    : IIntegrationTestUserAuthenticationService
{
    private string IntegrationTestUserName => settings.Value.IntegrationTestUserName;

    public string? CustomUserName { get; internal set; }

    public void SetUserName(string? customUserName) => this.CustomUserName = customUserName ?? this.IntegrationTestUserName;

    public void Reset() => this.CustomUserName = this.IntegrationTestUserName;

    public string GetUserName() => this.CustomUserName ?? this.IntegrationTestUserName;

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
