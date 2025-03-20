using System.Reflection;

using Automation.ServiceEnvironment.Services;

using Framework.DependencyInjection;
using Framework.DomainDriven.Auth;
using Framework.DomainDriven.Jobs;
using Framework.DomainDriven.WebApiNetCore;
using Framework.SecuritySystem.Credential;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Automation.ServiceEnvironment;

public static class ServiceProviderExtensions
{
    public static async Task RunJob<TJob>(this IServiceProvider rootServiceProvider, CancellationToken cancellationToken = default)
        where TJob : IJob
    {
        await rootServiceProvider.GetRequiredService<IJobServiceEvaluatorFactory>().RunJob<TJob>(cancellationToken);
    }

    public static ControllerEvaluator<TController> GetDefaultControllerEvaluator<TController>(
        this IServiceProvider rootServiceProvider,
        UserCredential? userCredential = null)
        where TController : ControllerBase
    {
        var controllerEvaluator = rootServiceProvider.GetRequiredService<ControllerEvaluator<TController>>();

        return userCredential == null ? controllerEvaluator : controllerEvaluator.WithImpersonate(userCredential);
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

    public static IServiceCollection RegisterJobs(
        this IServiceCollection services,
        Assembly[] assemblies)
    {
        foreach (var controllerType in assemblies.SelectMany(a => a.GetTypes())
                                                 .Where(t => !t.IsAbstract && typeof(IJob).IsAssignableFrom(t)))
        {
            services.AddScoped(controllerType);
        }

        return services;
    }

    public static IServiceCollection ApplyIntegrationTestServices(this IServiceCollection services) =>

        services.AddSingleton<IIntegrationTestUserAuthenticationService, IntegrationTestUserAuthenticationService>()
                .ReplaceSingletonFrom<IDefaultUserAuthenticationService, IIntegrationTestUserAuthenticationService>()

                .AddSingleton<IntegrationTestTimeProvider>()
                .ReplaceSingletonFrom<TimeProvider, IntegrationTestTimeProvider>()

                .AddScoped<TestWebApiCurrentMethodResolver>()
                .ReplaceScopedFrom<IWebApiCurrentMethodResolver, TestWebApiCurrentMethodResolver>()

                .ReplaceSingleton<IWebApiExceptionExpander, TestWebApiExceptionExpander>()

                .AddSingleton(typeof(ControllerEvaluator<>))

                .AddSingleton<RootAuthManager>()
                .AddSingleton(AdministratorsRoleList.Default)
                .AddScoped<AuthManager>();
}
