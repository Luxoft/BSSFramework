using Automation.ServiceEnvironment;
using Automation.ServiceEnvironment.Services;
using Automation.Settings;
using Automation.Xunit.ServiceEnvironment;

using Bss.Testing.Xunit.Interfaces;

using CommonFramework;
using CommonFramework.DependencyInjection;

using Framework.DomainDriven.WebApiNetCore;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using SecuritySystem.Testing;
using SecuritySystem.Testing.DependencyInjection;

namespace Automation.Xunit;

public static class DependencyInjection
{
    public static IServiceCollection AddIntegrationTestServices(
        this IServiceCollection services,
        Action<AutomationFrameworkSettings>? options = null)
    {
        if (options != null)
        {
            services.Configure(options);
        }

        return services
               .AddSingleton<ITestInitializeAndCleanup, DiTestInitializeAndCleanup>()

               .AddSingleton<IntegrationTestTimeProvider>()
               .ReplaceSingletonFrom<TimeProvider, IntegrationTestTimeProvider>()

               .AddScoped<TestWebApiCurrentMethodResolver>()
               .ReplaceScopedFrom<IWebApiCurrentMethodResolver, TestWebApiCurrentMethodResolver>()
               .ReplaceSingleton<IWebApiExceptionExpander, TestWebApiExceptionExpander>()

               .AddSingleton(typeof(ControllerEvaluator<>))

               .AddSecuritySystemTesting(b => b.SetEvaluator(typeof(BssTestingEvaluator<>))
                                               .SetTestRootUserInfo(sp => sp.GetRequiredService<IOptions<AutomationFrameworkSettings>>()
                                                                            .Pipe(o => new TestRootUserInfo(o.Value.IntegrationTestUserName))));
    }
}
