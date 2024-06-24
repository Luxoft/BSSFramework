namespace Automation.ServiceEnvironment.Services;

public static class IntegrationTestUserAuthenticationServiceExtensions
{
    public static async Task WithImpersonateAsync(
        this IIntegrationTestUserAuthenticationService service,
        string customUserName,
        Func<Task> action)
    {
        await service.WithImpersonateAsync(
            customUserName,
            async () =>
            {
                await action();
                return default(object);
            });
    }
}
