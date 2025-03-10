﻿using Automation.Interfaces;
using Automation.ServiceEnvironment;
using Automation.ServiceEnvironment.Services;
using Automation.Settings;

using Framework.DependencyInjection;
using Framework.DomainDriven.Auth;
using Framework.DomainDriven.WebApiNetCore;

using Microsoft.Extensions.DependencyInjection;

namespace Automation.Xunit;

public static class DependencyInjection
{
    public static IServiceCollection AddIntegrationTestServices(
        this IServiceCollection services,
        Action<AutomationFrameworkSettings> options) =>
        services.Configure(options)
                .AddSingleton<ITestInitializeAndCleanup, TestInitializeAndCleanup>()
                .AddSingleton<IIntegrationTestUserAuthenticationService, IntegrationTestUserAuthenticationService>()
                .ReplaceSingletonFrom<IDefaultUserAuthenticationService, IIntegrationTestUserAuthenticationService>()

                .AddSingleton<IntegrationTestTimeProvider>()
                .ReplaceSingletonFrom<TimeProvider, IntegrationTestTimeProvider>()

                .AddScoped<TestWebApiCurrentMethodResolver>()
                .ReplaceScopedFrom<IWebApiCurrentMethodResolver, TestWebApiCurrentMethodResolver>()
                .ReplaceSingleton<IWebApiExceptionExpander, TestWebApiExceptionExpander>()

                .AddSingleton(typeof(ControllerEvaluator<>))

                .AddScoped<AuthManager>();
}
