using Microsoft.Extensions.DependencyInjection;

namespace Anch.Testing;

public static class ServiceProviderExtensions
{
    public static async ValueTask RunEnvironmentHooks(this IServiceProvider serviceProvider, EnvironmentHookType hookType, CancellationToken ct)
    {
        var services = serviceProvider.GetKeyedServices<ITestEnvironmentHook>(hookType);

        foreach (var hook in hookType == EnvironmentHookType.Before ? services.Reverse() : services)
        {
            await hook.Process(ct);
        }
    }
}