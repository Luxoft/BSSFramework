using System.Reflection;

using CommonFramework;
using CommonFramework.Auth;
using CommonFramework.DependencyInjection;

using Framework.Application.Jobs;
using Framework.AutomationCore.ServiceEnvironment.ServiceEnvironment.Services;
using Framework.AutomationCore.Settings;
using Framework.Infrastructure.Middleware;
using Framework.Infrastructure.WebApiExceptionExpander;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

using SecuritySystem.Credential;
using SecuritySystem.Testing;
using SecuritySystem.Testing.DependencyInjection;

namespace Framework.AutomationCore.ServiceEnvironment.ServiceEnvironment;

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
        public IServiceCollection AddTestControllers(
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

        public IServiceCollection AddJobs(Assembly[] assemblies)
        {
            foreach (var controllerType in assemblies.SelectMany(a => a.GetTypes())
                                                     .Where(t => !t.IsAbstract && typeof(IJob).IsAssignableFrom(t)))
            {
                services.AddScoped(controllerType);
            }

            return services;
        }

        public IServiceCollection AddIntegrationTests(Action<AutomationFrameworkSettings>? setup = null)
        {
            if (setup != null)
            {
                services.Configure(setup);
            }

            return services.AddSingleton<IntegrationTestTimeProvider>()
                           .ReplaceSingletonFrom<TimeProvider, IntegrationTestTimeProvider>()

                           .AddScoped<TestWebApiCurrentMethodResolver>()
                           .ReplaceScopedFrom<IWebApiCurrentMethodResolver, TestWebApiCurrentMethodResolver>()
                           .ReplaceSingleton<IWebApiExceptionExpander, TestWebApiExceptionExpander>()

                           .AddSingleton(typeof(ControllerEvaluator<>))

                           .Replace(ServiceDescriptor.KeyedSingleton<ICurrentUser, TestingDefaultCurrentUser>(ICurrentUser.DefaultKey))

                           .AddSecuritySystemTesting(b => b.SetEvaluator(typeof(BssTestingEvaluator<>))
                                                           .SetTestRootUserInfo(sp => sp.GetRequiredService<IOptions<AutomationFrameworkSettings>>()
                                                                                        .Pipe(options => new TestRootUserInfo(options.Value.IntegrationTestUserName))));
        }
    }
}
