using Framework.Core.Services;
using Framework.DependencyInjection;
using Framework.DomainDriven.NHibernate.Audit;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.WebApiNetCore;

public static class UserAuthenticationDependencyInjectionExtensions
{
    public static IServiceCollection RegisterUserAuthenticationServices(this IServiceCollection services)
    {
        services.AddSingleton<ApplicationDefaultUserAuthenticationService>();
        services.AddSingletonFrom<IDefaultUserAuthenticationService, ApplicationDefaultUserAuthenticationService>();
        services.AddSingletonFrom<IAuditRevisionUserAuthenticationService, ApplicationDefaultUserAuthenticationService>();

        services.AddScoped<ApplicationUserAuthenticationService>();
        services.AddScopedFrom<IUserAuthenticationService, ApplicationUserAuthenticationService>();
        services.AddScopedFrom<IImpersonateService, ApplicationUserAuthenticationService>();

        return services;
    }
}
