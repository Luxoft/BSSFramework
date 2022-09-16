using System;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.WebApiNetCore;

public static class MiddlewareDependencyInjectionExtensions
{
    public static IServiceCollection RegisterMiddlewareServices(this IServiceCollection services)
    {
        services.AddSingleton<IWebApiExceptionExpander, WebApiExceptionExpander>();

        services.AddScoped<IWebApiDBSessionModeResolver, WebApiDBSessionModeResolver>();
        services.AddScoped<IWebApiCurrentMethodResolver, WebApiCurrentMethodResolver>();

        return services;
    }
}
