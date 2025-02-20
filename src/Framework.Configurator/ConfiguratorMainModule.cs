using Framework.Configurator.Handlers;
using Framework.Configurator.Interfaces;

using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Configurator;

public class ConfiguratorMainModule : IConfiguratorModule
{
    public string Name { get; } = "Main";

    public void AddServices(IServiceCollection services)
    {
        services.AddScoped<IGetOperationHandler, GetOperationHandler>()
                .AddScoped<IGetOperationsHandler, GetOperationsHandler>()
                .AddScoped<IGetBusinessRolesHandler, GetBusinessRolesHandler>()
                .AddScoped<IGetPrincipalsHandler, GetPrincipalsHandler>()
                .AddScoped<IGetRunAsHandler, GetRunAsHandler>()
                .AddScoped<IGetBusinessRoleContextsHandler, GetBusinessRoleContextsHandler>()
                .AddScoped<IGetBusinessRoleContextEntitiesHandler, GetBusinessRoleContextEntitiesHandler>()
                .AddScoped<IGetPrincipalHandler, GetPrincipalHandler>()
                .AddScoped<IGetBusinessRoleHandler, GetBusinessRoleHandler>()
                .AddScoped<IDownloadPermissionTemplateHandler, DownloadPermissionTemplateHandler>()
                .AddScoped<IGetModulesHandler, GetModulesHandler>()
                .AddScoped<ICreatePrincipalHandler, CreatePrincipalHandler>()
                .AddScoped<IUpdatePrincipalHandler, UpdatePrincipalHandler>()
                .AddScoped<IUpdatePermissionsHandler, UpdatePermissionsHandler>()
                .AddScoped<IDeletePrincipalHandler, DeletePrincipalHandler>()
                .AddScoped<IRunAsHandler, RunAsHandler>()
                .AddScoped<IStopRunAsHandler, StopRunAsHandler>();
    }

    public void MapApi(IEndpointRouteBuilder endpointsBuilder, string route)
    {
        endpointsBuilder.Get<IGetOperationsHandler>($"{route}/api/operations")
                        .Get<IGetOperationHandler>(route + "/api/operation/{name}")
                        .Get<IGetBusinessRolesHandler>($"{route}/api/roles")
                        .Get<IGetBusinessRoleContextsHandler>($"{route}/api/contexts")
                        .Get<IGetPrincipalsHandler>($"{route}/api/principals")
                        .Get<IGetBusinessRoleHandler>(route + "/api/role/{id}")
                        .Get<IGetPrincipalHandler>(route + "/api/principal/{id}")
                        .Get<IGetBusinessRoleContextEntitiesHandler>(route + "/api/context/{id}/entities")
                        .Get<IGetRunAsHandler>($"{route}/api/principal/current/runAs")
                        .Get<IGetModulesHandler>($"{route}/api/modules")
                        .Get<IDownloadPermissionTemplateHandler>($"{route}/api/permissions/template")
                        .Post<ICreatePrincipalHandler>($"{route}/api/principals")
                        .Post<IUpdatePrincipalHandler>(route + "/api/principal/{id}")
                        .Post<IUpdatePermissionsHandler>(route + "/api/principal/{id}/permissions")
                        .Post<IRunAsHandler>($"{route}/api/principal/current/runAs")
                        .Delete<IStopRunAsHandler>($"{route}/api/principal/current/runAs")
                        .Delete<IDeletePrincipalHandler>(route + "/api/principal/{id}");
    }
}
