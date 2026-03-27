using Framework.Infrastructure.WebApiExceptionExpander;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Infrastructure.Middleware;

public static class MiddlewareDependencyInjectionExtensions
{
    public static IServiceCollection RegisterMiddlewareServices(this IServiceCollection services)
    {
        services.AddScoped<IWebApiExceptionExpander, WebApiExceptionExpander.WebApiExceptionExpander>();

        services.AddScoped<IWebApiDBSessionModeResolver, WebApiDBSessionModeResolver>();
        services.AddScoped<IWebApiCurrentMethodResolver, WebApiCurrentMethodResolver>();

        return services;
    }
}
