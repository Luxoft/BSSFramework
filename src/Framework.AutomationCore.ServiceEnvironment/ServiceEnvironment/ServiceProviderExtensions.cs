using System.Reflection;

using Automation.ServiceEnvironment.Services;

using Framework.Core.Services;
using Framework.DependencyInjection;
using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate.Audit;
using Framework.DomainDriven.WebApiNetCore;

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

    public static IServiceCollection ApplyIntegrationTestServices(this IServiceCollection services)
    {
        return services.AddSingleton<IntegrationTestUserAuthenticationService>()
                       .ReplaceSingletonFrom<IAuditRevisionUserAuthenticationService, IntegrationTestUserAuthenticationService>()
                       .ReplaceSingletonFrom<IDefaultUserAuthenticationService, IntegrationTestUserAuthenticationService>()

                       .AddSingleton<IntegrationTestDateTimeService>()
                       .ReplaceSingletonFrom<IDateTimeService, IntegrationTestDateTimeService>()

                       .AddScoped<TestWebApiCurrentMethodResolver>()
                       .ReplaceScopedFrom<IWebApiCurrentMethodResolver, TestWebApiCurrentMethodResolver>()

                       .ReplaceSingleton<IWebApiExceptionExpander, TestWebApiExceptionExpander>();;
    }
}
