using Framework.DomainDriven.WebApiNetCore;
using Framework.DomainDriven.WebApiNetCore.Integration;
using Framework.Infrastructure.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBssFramework(this IServiceCollection services, Action<IBssFrameworkSettings>? setupAction)
    {
        services.RegisterGenericServices();
        services.RegisterDefaultUserAuthenticationServices();
        services.RegisterWebApiGenericServices();

        var settings = new BssFrameworkSettings();

        setupAction?.Invoke(settings);

        settings.Initialize(services);

        return services;
    }
    public static IServiceCollection RegisterWebApiGenericServices(this IServiceCollection services) =>
        services.RegisterMiddlewareServices()
                .RegisterXsdExport();

    private static IServiceCollection RegisterXsdExport(this IServiceCollection services) =>
        services.AddSingleton<IEventXsdExporter2, EventXsdExporter2>();
}
