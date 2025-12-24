using SecuritySystem.Credential;
using SecuritySystem.Testing;

namespace Automation.ServiceEnvironment.Services;

public static class TestingUserAuthenticationServiceExtensions
{
    public static async Task WithImpersonateAsync(
        this ITestingUserAuthenticationService service,
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
