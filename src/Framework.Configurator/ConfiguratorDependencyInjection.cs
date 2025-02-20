using System.Reflection;

using Framework.Configurator.Interfaces;
using Framework.Core;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace Framework.Configurator;

public static class ConfiguratorDependencyInjection
{
    private const string EmbeddedFileNamespace = "Framework.Configurator.configurator_ui.dist";

    public static IServiceCollection AddConfigurator(this IServiceCollection services, Action<IConfiguratorSetup>? setupAction = null)
    {
        var configurationSetup = new ConfiguratorSetup();

        configurationSetup.AddModule(new ConfiguratorMainModule());
        setupAction?.Invoke(configurationSetup);

        configurationSetup.Initialize(services);

        return services;
    }

    public static IApplicationBuilder UseConfigurator(this IApplicationBuilder app, string route = "/admin/configurator") =>
        app
            .UseMiddleware<ConfiguratorMiddleware>(route)
            .UseStaticFiles(
                new StaticFileOptions
                {
                    RequestPath = route,
                    FileProvider = new EmbeddedFileProvider(
                        typeof(ConfiguratorDependencyInjection).GetTypeInfo().Assembly,
                        EmbeddedFileNamespace)
                })
            .UseRouting() // needed for IIS
            .UseEndpoints(x => x.MapApi(route));

    private static void MapApi(this IEndpointRouteBuilder endpointsBuilder, string route) =>
        endpointsBuilder.ServiceProvider.GetRequiredService<IEnumerable<IConfiguratorModule>>()
                        .Foreach(module => module.MapApi(endpointsBuilder, route));

    public static IEndpointRouteBuilder Get<THandler>(this IEndpointRouteBuilder builder, string pattern)
        where THandler : IHandler
    {
        builder.MapGet(pattern, async x => await x.RequestServices.GetRequiredService<THandler>().Execute(x, x.RequestAborted));
        return builder;
    }

    public static IEndpointRouteBuilder Post<THandler>(this IEndpointRouteBuilder builder, string pattern)
        where THandler : IHandler
    {
        builder.MapPost(pattern, async x => await x.RequestServices.GetRequiredService<THandler>().Execute(x, x.RequestAborted));
        return builder;
    }

    public static IEndpointRouteBuilder Delete<THandler>(this IEndpointRouteBuilder builder, string pattern)
        where THandler : IHandler
    {
        builder.MapDelete(pattern, async x => await x.RequestServices.GetRequiredService<THandler>().Execute(x, x.RequestAborted));
        return builder;
    }
}
