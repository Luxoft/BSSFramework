using Framework.DomainDriven.WebApiNetCore.Auth;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.Auth;

public static class UserAuthenticationDependencyInjectionExtensions
{
    public static IServiceCollection RegisterDefaultUserAuthenticationServices(this IServiceCollection services)
    {
        services.AddSingleton<IApplicationDefaultUserAuthenticationServiceSettings, ApplicationDefaultUserAuthenticationServiceSettings>();
        services.AddSingleton<IDefaultUserAuthenticationService, ApplicationDefaultUserAuthenticationService>();

        return services;
    }
}
