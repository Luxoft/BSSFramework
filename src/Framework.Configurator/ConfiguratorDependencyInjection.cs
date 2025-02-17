﻿using System.Reflection;

using Framework.Configurator.Handlers;
using Framework.Configurator.Interfaces;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace Framework.Configurator;

public static class ConfiguratorDependencyInjection
{
    private const string EmbeddedFileNamespace = "Framework.Configurator.configurator_ui.dist";

    public static IServiceCollection AddConfigurator(this IServiceCollection services)
        => services
           .AddScoped<IGetSystemConstantsHandler, GetSystemConstantsHandler>()
           .AddScoped<IGetDomainTypesHandler, GetDomainTypesHandler>()
           .AddScoped<IGetOperationHandler, GetOperationHandler>()
           .AddScoped<IGetOperationsHandler, GetOperationsHandler>()
           .AddScoped<IGetBusinessRolesHandler, GetBusinessRolesHandler>()
           .AddScoped<IGetPrincipalsHandler, GetPrincipalsHandler>()
           .AddScoped<IGetRunAsHandler, GetRunAsHandler>()
           .AddScoped<IGetBusinessRoleContextsHandler, GetBusinessRoleContextsHandler>()
           .AddScoped<IGetBusinessRoleContextEntitiesHandler, GetBusinessRoleContextEntitiesHandler>()
           .AddScoped<IGetPrincipalHandler, GetPrincipalHandler>()
           .AddScoped<IGetBusinessRoleHandler, GetBusinessRoleHandler>()
           .AddScoped<ICreatePrincipalHandler, CreatePrincipalHandler>()
           .AddScoped<IUpdateSystemConstantHandler, UpdateSystemConstantHandler>()
           .AddScoped<IUpdatePrincipalHandler, UpdatePrincipalHandler>()
           .AddScoped<IUpdatePermissionsHandler, UpdatePermissionsHandler>()
           .AddScoped<IForcePushEventHandler, ForcePushEventHandler>()
           .AddScoped<IDeletePrincipalHandler, DeletePrincipalHandler>()
           .AddScoped<IRunAsHandler, RunAsHandler>()
           .AddScoped<IStopRunAsHandler, StopRunAsHandler>()
           .AddScoped<IDownloadPermissionTemplateHandler, DownloadPermissionTemplateHandler>();

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
        endpointsBuilder
            .Get<IGetSystemConstantsHandler>($"{route}/api/constants")
            .Get<IGetDomainTypesHandler>($"{route}/api/domainTypes")
            .Get<IGetOperationsHandler>($"{route}/api/operations")
            .Get<IGetOperationHandler>(route + "/api/operation/{name}")
            .Get<IGetBusinessRolesHandler>($"{route}/api/roles")
            .Get<IGetBusinessRoleContextsHandler>($"{route}/api/contexts")
            .Get<IGetPrincipalsHandler>($"{route}/api/principals")
            .Get<IGetBusinessRoleHandler>(route + "/api/role/{id}")
            .Get<IGetPrincipalHandler>(route + "/api/principal/{id}")
            .Get<IGetBusinessRoleContextEntitiesHandler>(route + "/api/context/{id}/entities")
            .Get<IGetRunAsHandler>($"{route}/api/principal/current/runAs")
            .Get<IDownloadPermissionTemplateHandler>($"{route}/api/permissions/template")
            .Post<ICreatePrincipalHandler>($"{route}/api/principals")
            .Post<IUpdateSystemConstantHandler>(route + "/api/constant/{id}")
            .Post<IUpdatePrincipalHandler>(route + "/api/principal/{id}")
            .Post<IUpdatePermissionsHandler>(route + "/api/principal/{id}/permissions")
            .Post<IForcePushEventHandler>(route + "/api/domainType/{domainTypeName}")
            .Post<IRunAsHandler>($"{route}/api/principal/current/runAs")
            .Delete<IStopRunAsHandler>($"{route}/api/principal/current/runAs")
            .Delete<IDeletePrincipalHandler>(route + "/api/principal/{id}");

    private static IEndpointRouteBuilder Get<THandler>(this IEndpointRouteBuilder builder, string pattern)
        where THandler : IHandler
    {
        builder.MapGet(pattern, async x => await x.RequestServices.GetRequiredService<THandler>().Execute(x, x.RequestAborted));
        return builder;
    }

    private static IEndpointRouteBuilder Post<THandler>(this IEndpointRouteBuilder builder, string pattern)
        where THandler : IHandler
    {
        builder.MapPost(pattern, async x => await x.RequestServices.GetRequiredService<THandler>().Execute(x, x.RequestAborted));
        return builder;
    }

    private static IEndpointRouteBuilder Delete<THandler>(this IEndpointRouteBuilder builder, string pattern)
        where THandler : IHandler
    {
        builder.MapDelete(pattern, async x => await x.RequestServices.GetRequiredService<THandler>().Execute(x, x.RequestAborted));
        return builder;
    }
}
