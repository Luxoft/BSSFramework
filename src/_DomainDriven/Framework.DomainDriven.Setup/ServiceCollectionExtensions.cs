using Framework.DomainDriven.ApplicationCore;
using Framework.DomainDriven.WebApiNetCore;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.Setup;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBssFramework(this IServiceCollection services, Action<IBssFrameworkSettings>? setupAction)
    {
        services.RegisterGenericServices();
        services.RegisterWebApiGenericServices();

        var settings = new BssFrameworkSettings();

        setupAction?.Invoke(settings);

        settings.Initialize(services);

        return services;
    }
}
