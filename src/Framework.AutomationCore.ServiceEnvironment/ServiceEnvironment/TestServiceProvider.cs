using System;

using Framework.DependencyInjection;
using Framework.DomainDriven.WebApiNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Automation.ServiceEnvironment;

public static class TestServiceProvider
{
    public static IServiceProvider Build(Action<IServiceCollection> options)
    {
        var serviceCollection = new ServiceCollection();
        options(serviceCollection);

        serviceCollection
            .AddTestDateTimeService()
            .ReplaceScopedFrom<IWebApiCurrentMethodResolver, TestWebApiCurrentMethodResolver>()
            .ReplaceSingleton<IWebApiExceptionExpander, WebApiDebugExceptionExpander>()
            .AddScoped<TestWebApiCurrentMethodResolver>()
            .AddTestAuthentication();

        return serviceCollection
            .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });
    }
}
