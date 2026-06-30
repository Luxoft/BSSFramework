using System.Reflection;

using Anch.Core;
using Anch.DependencyInjection;
using Anch.SecuritySystem.Testing;
using Anch.SecuritySystem.Testing.DependencyInjection;

using Framework.Application.Jobs;
using Framework.AutomationCore.Services;
using Framework.Infrastructure.Middleware;
using Framework.Infrastructure.WebApiExceptionExpander;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Framework.AutomationCore.Extensions;

public static class ServiceCollectionExtensions
{
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
            if (setup is not null)
            {
                services.Configure(setup);
            }

            return services.AddSingleton<IntegrationTestTimeProvider>()
                           .ReplaceSingletonFrom<TimeProvider, IntegrationTestTimeProvider>()

                           .AddScoped<TestWebApiCurrentMethodResolver>()
                           .ReplaceScopedFrom<IWebApiCurrentMethodResolver, TestWebApiCurrentMethodResolver>()
                           .ReplaceSingleton<IWebApiExceptionExpander, TestWebApiExceptionExpander>()

                           .AddSingleton(typeof(ControllerEvaluator<>))

                           .AddSecuritySystemTesting(b => b.SetEvaluator(typeof(BssTestingEvaluator<>))
                                                           .SetTestRootUserInfo(sp => sp.GetRequiredService<IOptions<AutomationFrameworkSettings>>()
                                                                                        .Pipe(options => new TestRootUserInfo(
                                                                                                  options.Value.IntegrationTestUserName))));
        }
    }
}

