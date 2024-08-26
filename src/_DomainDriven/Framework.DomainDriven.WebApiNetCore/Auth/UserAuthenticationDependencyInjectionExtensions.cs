using Framework.DependencyInjection;
using Framework.DomainDriven.Auth;
using Framework.DomainDriven.NHibernate.Audit;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.WebApiNetCore.Auth;

public static class UserAuthenticationDependencyInjectionExtensions
{
    public static IServiceCollection RegisterDefaultUserAuthenticationServices(this IServiceCollection services)
    {
        services.AddSingleton<IApplicationDefaultUserAuthenticationServiceSettings, ApplicationDefaultUserAuthenticationServiceSettings>();
        services.AddSingleton<ApplicationDefaultUserAuthenticationService>();
        services.AddSingletonFrom<IDefaultUserAuthenticationService, ApplicationDefaultUserAuthenticationService>();
        services.AddSingletonFrom<IAuditRevisionUserAuthenticationService, ApplicationDefaultUserAuthenticationService>();

        return services;
    }
}
