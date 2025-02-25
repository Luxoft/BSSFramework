using Framework.DomainDriven.Auth;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.WebApiNetCore.Auth;

public static class UserAuthenticationDependencyInjectionExtensions
{
    public static IServiceCollection RegisterDefaultUserAuthenticationServices(this IServiceCollection services)
    {
        services.AddSingleton<IApplicationDefaultUserAuthenticationServiceSettings, ApplicationDefaultUserAuthenticationServiceSettings>();
        services.AddSingleton<IDefaultUserAuthenticationService, ApplicationDefaultUserAuthenticationService>();

        return services;
    }
}
