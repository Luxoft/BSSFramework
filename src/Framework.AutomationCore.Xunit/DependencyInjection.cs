using Automation.ServiceEnvironment;
using Automation.ServiceEnvironment.Services;
using Automation.Settings;
using Automation.Xunit.ServiceEnvironment;

using Bss.Testing.Xunit.Interfaces;

using Framework.DomainDriven.Auth;
using Framework.DomainDriven.WebApiNetCore;

using Microsoft.Extensions.DependencyInjection;

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
            .AddSingleton<IIntegrationTestUserAuthenticationService, IntegrationTestUserAuthenticationService>()
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
}
