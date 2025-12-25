using Framework.DomainDriven.WebApiNetCore.Integration;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.WebApiNetCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterWebApiGenericServices(this IServiceCollection services) =>
        services.RegisterMiddlewareServices()
                .RegisterXsdExport();

    private static IServiceCollection RegisterXsdExport(this IServiceCollection services) =>
        services.AddSingleton<IEventXsdExporter2, EventXsdExporter2>();
}
