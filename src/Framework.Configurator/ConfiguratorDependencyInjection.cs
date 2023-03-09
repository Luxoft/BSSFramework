using System.Reflection;

using Framework.Authorization.BLL;
using Framework.Configuration.BLL;
using Framework.Configurator.Handlers;
using Framework.Configurator.Interfaces;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace Framework.Configurator
{
    public static class ConfiguratorDependencyInjection
    {
        private const string EmbeddedFileNamespace = "Framework.Configurator.configurator_ui.dist";

        public static IServiceCollection AddConfigurator<TEnvironment, TBllContext>(this IServiceCollection services)
            where TEnvironment : IContextEvaluator<TBllContext>
            where TBllContext : DomainDriven.BLL.Configuration.IConfigurationBLLContextContainer<IConfigurationBLLContext>,
            IAuthorizationBLLContextContainer<IAuthorizationBLLContext> =>
            services
                .AddScoped<IGetSystemConstantsHandler, GetSystemConstantsHandler<TBllContext>>()
                .AddScoped<IGetDomainTypesHandler, GetDomainTypesHandler<TBllContext>>()
                .AddScoped<IGetOperationHandler, GetOperationHandler<TBllContext>>()
                .AddScoped<IGetOperationsHandler, GetOperationsHandler<TBllContext>>()
                .AddScoped<IGetBusinessRolesHandler, GetBusinessRolesHandler<TBllContext>>()
                .AddScoped<IGetPrincipalsHandler, GetPrincipalsHandler<TBllContext>>()
                .AddScoped<IGetRunAsHandler, GetRunAsHandler<TBllContext>>()
                .AddScoped<IGetBusinessRoleContextsHandler, GetBusinessRoleContextsHandler<TBllContext>>()
                .AddScoped<IGetBusinessRoleContextEntitiesHandler, GetBusinessRoleContextEntitiesHandler<TBllContext>>()
                .AddScoped<IGetPrincipalHandler, GetPrincipalHandler<TBllContext>>()
                .AddScoped<IGetBusinessRoleHandler, GetBusinessRoleHandler<TBllContext>>()
                .AddScoped<ICreateBusinessRoleHandler, CreateBusinessRoleHandler<TBllContext>>()
                .AddScoped<ICreatePrincipalHandler, CreatePrincipalHandler<TBllContext>>()
                .AddScoped<IUpdateSystemConstantHandler, UpdateSystemConstantHandler<TBllContext>>()
                .AddScoped<IUpdateBusinessRoleHandler, UpdateBusinessRoleHandler<TBllContext>>()
                .AddScoped<IUpdatePrincipalHandler, UpdatePrincipalHandler<TBllContext>>()
                .AddScoped<IUpdatePermissionsHandler, UpdatePermissionsHandler<TBllContext>>()
                .AddScoped<IForcePushEventHandler, ForcePushEventHandler<TBllContext>>()
                .AddScoped<IDeleteBusinessRoleHandler, DeleteBusinessRoleHandler<TBllContext>>()
                .AddScoped<IDeletePrincipalHandler, DeletePrincipalHandler<TBllContext>>()
                .AddScoped<IRunAsHandler, RunAsHandler<TBllContext>>()
                .AddScoped<IStopRunAsHandler, StopRunAsHandler<TBllContext>>()
                .AddSingleton<IContextEvaluator<TBllContext>>(x => x.GetRequiredService<TEnvironment>());

        public static IApplicationBuilder UseConfigurator(this IApplicationBuilder app, string route = "/admin/configurator") =>
            app
                .UseMiddleware<ConfiguratorMiddleware>(route)
                .UseFileServer(
                    new FileServerOptions
                    {
                        RequestPath = route,
                        FileProvider = new EmbeddedFileProvider(
                            typeof(ConfiguratorDependencyInjection).GetTypeInfo().Assembly,
                            EmbeddedFileNamespace)
                    })
                .UseEndpoints(x => x.MapApi(route));

        private static void MapApi(this IEndpointRouteBuilder endpointsBuilder, string route) =>
            endpointsBuilder
                .Get<IGetSystemConstantsHandler>($"{route}/api/constants")
                .Get<IGetDomainTypesHandler>($"{route}/api/domainTypes")
                .Get<IGetOperationsHandler>($"{route}/api/operations")
                .Get<IGetOperationHandler>(route + "/api/operation/{id}")
                .Get<IGetBusinessRolesHandler>($"{route}/api/roles")
                .Get<IGetBusinessRoleContextsHandler>($"{route}/api/contexts")
                .Get<IGetPrincipalsHandler>($"{route}/api/principals")
                .Get<IGetBusinessRoleHandler>(route + "/api/role/{id}")
                .Get<IGetPrincipalHandler>(route + "/api/principal/{id}")
                .Get<IGetBusinessRoleContextEntitiesHandler>(route + "/api/context/{id}/entities")
                .Get<IGetRunAsHandler>($"{route}/api/principal/current/runAs")
                .Post<ICreateBusinessRoleHandler>($"{route}/api/roles")
                .Post<ICreatePrincipalHandler>($"{route}/api/principals")
                .Post<IUpdateSystemConstantHandler>(route + "/api/constant/{id}")
                .Post<IUpdateBusinessRoleHandler>(route + "/api/role/{id}")
                .Post<IUpdatePrincipalHandler>(route + "/api/principal/{id}")
                .Post<IUpdatePermissionsHandler>(route + "/api/principal/{id}/permissions")
                .Post<IForcePushEventHandler>(route + "/api/domainType/{id}/operation/{operationId}")
                .Post<IRunAsHandler>($"{route}/api/principal/current/runAs")
                .Delete<IStopRunAsHandler>($"{route}/api/principal/current/runAs")
                .Delete<IDeletePrincipalHandler>(route + "/api/principal/{id}")
                .Delete<IDeleteBusinessRoleHandler>(route + "/api/role/{id}");

        private static IEndpointRouteBuilder Get<THandler>(this IEndpointRouteBuilder endpointsBuilder, string pattern)
            where THandler : IHandler
        {
            endpointsBuilder.MapGet(pattern, async x => await x.RequestServices.GetRequiredService<THandler>().Execute(x))
                            .RequireAuthorization();
            return endpointsBuilder;
        }

        private static IEndpointRouteBuilder Post<THandler>(this IEndpointRouteBuilder endpointsBuilder, string pattern)
            where THandler : IHandler
        {
            endpointsBuilder.MapPost(pattern, async x => await x.RequestServices.GetRequiredService<THandler>().Execute(x))
                            .RequireAuthorization();
            return endpointsBuilder;
        }

        private static IEndpointRouteBuilder Delete<THandler>(this IEndpointRouteBuilder endpointsBuilder, string pattern)
            where THandler : IHandler
        {
            endpointsBuilder.MapDelete(pattern, async x => await x.RequestServices.GetRequiredService<THandler>().Execute(x))
                            .RequireAuthorization();
            return endpointsBuilder;
        }
    }
}
