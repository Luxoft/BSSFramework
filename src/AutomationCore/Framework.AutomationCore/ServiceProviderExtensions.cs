using Anch.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.AutomationCore;

public static class ServiceProviderExtensions
{
    public static IServiceProvider BuildDefaultServiceProvider(this IServiceCollection services) =>
        services.AddValidator<DuplicateServiceUsageValidator>()
                .BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true, ValidateOnBuild = true });
}
