using System.Reflection;

using Automation.ServiceEnvironment.Services;

using CommonFramework.DependencyInjection;

using Framework.DomainDriven.Auth;
using Framework.DomainDriven.Jobs;
using Framework.DomainDriven.WebApiNetCore;
using SecuritySystem.Credential;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using SecuritySystem.Testing.DependencyInjection;

namespace Automation.ServiceEnvironment;

public static class ServiceProviderExtensions
{
    extension(IServiceProvider rootServiceProvider)
    {
        public async Task RunJob<TJob>(CancellationToken cancellationToken = default)
            where TJob : IJob
        {
            await rootServiceProvider.GetRequiredService<IJobServiceEvaluatorFactory>().RunJob<TJob>(cancellationToken);
        }

        public ControllerEvaluator<TController> GetDefaultControllerEvaluator<TController>(UserCredential? userCredential = null)
            where TController : ControllerBase
        {
            var controllerEvaluator = rootServiceProvider.GetRequiredService<ControllerEvaluator<TController>>();

            return userCredential == null ? controllerEvaluator : controllerEvaluator.WithImpersonate(userCredential);
        }
    }

    extension(IServiceCollection services)
    {
        public IServiceCollection RegisterControllers(
            Assembly[] assemblies,
            params Type[] exceptControllers)
        {
            foreach (var controllerType in assemblies.SelectMany(a => a.GetTypes())
                                                     .Except(exceptControllers)
                                                     .Where(t => !t.IsAbstract && typeof(ControllerBase).IsAssignableFrom(t)))
            {
                services.AddScoped(controllerType);
            }

            return services;
        }

        public IServiceCollection RegisterJobs(Assembly[] assemblies)
        {
            foreach (var controllerType in assemblies.SelectMany(a => a.GetTypes())
                                                     .Where(t => !t.IsAbstract && typeof(IJob).IsAssignableFrom(t)))
            {
                services.AddScoped(controllerType);
            }

            return services;
        }

        public IServiceCollection ApplyIntegrationTestServices() =>

            services.AddSingleton<IIntegrationTestUserAuthenticationService, IntegrationTestUserAuthenticationService>()
                    .ReplaceSingletonFrom<IDefaultUserAuthenticationService, IIntegrationTestUserAuthenticationService>()

                    .AddSingleton<IntegrationTestTimeProvider>()
                    .ReplaceSingletonFrom<TimeProvider, IntegrationTestTimeProvider>()

                    .AddScoped<TestWebApiCurrentMethodResolver>()
                    .ReplaceScopedFrom<IWebApiCurrentMethodResolver, TestWebApiCurrentMethodResolver>()

                    .ReplaceSingleton<IWebApiExceptionExpander, TestWebApiExceptionExpander>()

                    .AddSingleton(typeof(ControllerEvaluator<>))

                    .AddSecuritySystemTesting();
    }
}
