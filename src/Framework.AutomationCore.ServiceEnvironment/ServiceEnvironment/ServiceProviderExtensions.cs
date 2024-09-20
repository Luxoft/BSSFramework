using System.Reflection;

using Automation.ServiceEnvironment.Services;

using Framework.DependencyInjection;
using Framework.DomainDriven.Auth;
using Framework.DomainDriven.Jobs;
using Framework.DomainDriven.NHibernate.Audit;
using Framework.DomainDriven.WebApiNetCore;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Automation.ServiceEnvironment;

public static class ServiceProviderExtensions
{
    public static async Task RunJob<TJob>(this IServiceProvider rootServiceProvider, CancellationToken cancellationToken = default)
        where TJob : IJob
    {
        await rootServiceProvider.GetRequiredService<IJobEvaluator>().RunJob<TJob>(cancellationToken);
    }

    public static ControllerEvaluator<TController> GetDefaultControllerEvaluator<TController>(
        this IServiceProvider rootServiceProvider,
        string? principalName = null)
        where TController : ControllerBase
    {
        var controllerEvaluator = rootServiceProvider.GetRequiredService<ControllerEvaluator<TController>>();

        return principalName == null ? controllerEvaluator : controllerEvaluator.WithImpersonate(principalName);
    }

    public static IServiceCollection RegisterControllers(
        this IServiceCollection services,
        Assembly[] assemblies,
        params Type[] exceptControllers)
    {
        foreach (var controllerType in assemblies.SelectMany(
                                                     a => a.GetTypes())
                                                 .Except(exceptControllers)
                                                 .Where(t => !t.IsAbstract && typeof(ControllerBase).IsAssignableFrom(t)))
        {
            services.AddScoped(controllerType);
        }

        return services;
    }

    public static IServiceCollection ApplyIntegrationTestServices(this IServiceCollection services) =>

        services.AddSingleton<IIntegrationTestUserAuthenticationService, IntegrationTestUserAuthenticationService>()
                .ReplaceSingletonFrom<IAuditRevisionUserAuthenticationService, IIntegrationTestUserAuthenticationService>()
                .ReplaceSingletonFrom<IDefaultUserAuthenticationService, IIntegrationTestUserAuthenticationService>()

                .AddSingleton<IntegrationTestTimeProvider>()
                .ReplaceSingletonFrom<TimeProvider, IntegrationTestTimeProvider>()

                .AddScoped<TestWebApiCurrentMethodResolver>()
                .ReplaceScopedFrom<IWebApiCurrentMethodResolver, TestWebApiCurrentMethodResolver>()

                .ReplaceSingleton<IWebApiExceptionExpander, TestWebApiExceptionExpander>()

                .AddSingleton(typeof(ControllerEvaluator<>))

                .AddScoped<AuthManager>();
}
