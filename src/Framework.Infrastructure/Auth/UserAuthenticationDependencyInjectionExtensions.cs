using Framework.Application.Auth;
using Framework.Core.Auth;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Infrastructure.Auth;

public static class UserAuthenticationDependencyInjectionExtensions
{
    public static IServiceCollection RegisterDefaultUserAuthenticationServices(this IServiceCollection services)
    {
        services.AddSingleton(ApplicationDefaultUserAuthenticationServiceSettings.Default);
        services.AddSingleton<IDefaultUserAuthenticationService, ApplicationDefaultUserAuthenticationService>();

        return services;
    }
}
