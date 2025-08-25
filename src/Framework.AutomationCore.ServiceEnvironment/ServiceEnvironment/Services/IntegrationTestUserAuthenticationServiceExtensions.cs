using SecuritySystem.Credential;

namespace Automation.ServiceEnvironment.Services;

public static class IntegrationTestUserAuthenticationServiceExtensions
{
    public static async Task WithImpersonateAsync(
        this IIntegrationTestUserAuthenticationService service,
        UserCredential customUserCredential,
        Func<Task> action)
    {
        await service.WithImpersonateAsync(
            customUserCredential,
            async () =>
            {
                await action();
                return default(object);
            });
    }
}
