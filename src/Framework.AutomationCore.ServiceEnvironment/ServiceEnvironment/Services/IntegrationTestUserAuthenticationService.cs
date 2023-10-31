namespace Automation.ServiceEnvironment.Services;

public class IntegrationTestUserAuthenticationService : IIntegrationTestUserAuthenticationService
{
    private readonly string integrationTestUserName;

    public IntegrationTestUserAuthenticationService(string integrationTestUserName = "IntegrationTestRootUser") =>
        this.integrationTestUserName = integrationTestUserName;

    public string CustomUserName { get; internal set; }

    public void SetUserName(string customUserName) => this.CustomUserName = customUserName ?? this.integrationTestUserName;

    public void Reset() => this.CustomUserName = this.integrationTestUserName;

    public string GetUserName() => this.CustomUserName ?? this.integrationTestUserName;

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
