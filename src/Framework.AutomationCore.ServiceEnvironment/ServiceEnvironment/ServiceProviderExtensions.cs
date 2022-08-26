using System;
using System.Linq;
using System.Reflection;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Automation.ServiceEnvironment;

public static class ServiceProviderExtensions
{
    public static ControllerEvaluator<TController> GetDefaultControllerEvaluator<TController>(
        this IServiceProvider serviceProvider,
        string principalName = null)
            where TController : ControllerBase
    {
        var controllerEvaluator = serviceProvider.GetRequiredService<ControllerEvaluator<TController>>();

        return principalName == null ? controllerEvaluator : controllerEvaluator.WithImpersonate(principalName);
    }

    public static IServiceCollection RegisterControllers(this IServiceCollection services, Assembly[] assemblies, params Type[] exceptControllers)
    {
        foreach (var controllerType in assemblies.SelectMany(
                     a => a.GetTypes())
                     .Except(exceptControllers)
                     .Where(t => !t.IsAbstract && typeof(ControllerBase).IsAssignableFrom(t)))
        {
            services.AddScoped(controllerType);

            services.AddSingleton(typeof(ControllerEvaluator<>).MakeGenericType(controllerType));
        }

        return services;
    }
}
