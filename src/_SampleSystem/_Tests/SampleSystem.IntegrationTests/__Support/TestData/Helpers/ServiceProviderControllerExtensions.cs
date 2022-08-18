using System;
using System.Linq;
using Automation.ServiceEnvironment;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace SampleSystem.IntegrationTests.__Support.TestData.Helpers;

public static class ServiceProviderControllerExtensions
{
    public static IServiceCollection RegisterControllers(this IServiceCollection services)
    {
        var asms = new[]
                   {
                           typeof(SampleSystem.WebApiCore.Controllers.Main.EmployeeController).Assembly,
                   };

        var exceptControllers = new Type[]
                                {
                                };


        foreach (var controllerType in asms.SelectMany(a => a.GetTypes()).Except(exceptControllers).Where(t => !t.IsAbstract && typeof(ControllerBase).IsAssignableFrom(t)))
        {
            services.AddScoped(controllerType);

            services.AddSingleton(typeof(ControllerEvaluator<>).MakeGenericType(controllerType));
        }

        return services;
    }
}
