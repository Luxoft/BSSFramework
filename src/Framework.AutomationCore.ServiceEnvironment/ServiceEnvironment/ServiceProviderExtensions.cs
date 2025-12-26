using System.Reflection;

using Automation.ServiceEnvironment.Services;
using Automation.Settings;

using CommonFramework;
using CommonFramework.DependencyInjection;

using Framework.DomainDriven.Auth;
using Framework.DomainDriven.Jobs;
using Framework.DomainDriven.WebApiNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using SecuritySystem.Credential;
using SecuritySystem.Services;
using SecuritySystem.Testing;
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

            services.AddSingleton<IntegrationTestTimeProvider>()
                    .ReplaceSingletonFrom<TimeProvider, IntegrationTestTimeProvider>()

                    .AddScoped<TestWebApiCurrentMethodResolver>()
                    .ReplaceScopedFrom<IWebApiCurrentMethodResolver, TestWebApiCurrentMethodResolver>()

                    .ReplaceSingleton<IWebApiExceptionExpander, TestWebApiExceptionExpander>()

                    .AddSingleton(typeof(ControllerEvaluator<>))

                    .AddSecuritySystemTesting(b => b.SetEvaluator(typeof(BssTestingEvaluator<>))
                                                    .SetTestRootUserInfo(sp => sp.GetRequiredService<IOptions<AutomationFrameworkSettings>>()
                                                                                 .Pipe(options => new TestRootUserInfo(options.Value.IntegrationTestUserName))));
    }
}

public class BssTestingUserAuthenticationService(ITestingEvaluator<IUserCredentialNameResolver> credentialNameResolverEvaluator, TestRootUserInfo testRootUserInfo)
    : TestingUserAuthenticationService(credentialNameResolverEvaluator, testRootUserInfo), IImpersonateService
{

};
