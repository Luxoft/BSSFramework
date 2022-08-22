using System;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.WebApiNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Automation.ServiceEnvironment;

public static class TestServiceProvider
{
    public static IServiceProvider Build(Action<IServiceCollection> options)
    {
        var serviceCollection = new ServiceCollection()
            .AddTestAuthentication()
            .AddTestDateTimeService()
            .ReplaceScopedFrom<IWebApiCurrentMethodResolver, TestWebApiCurrentMethodResolver>()
            .ReplaceSingleton<IWebApiExceptionExpander, WebApiDebugExceptionExpander>()
            .AddScoped<TestWebApiCurrentMethodResolver>();

        options(serviceCollection);
        return serviceCollection
            .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });
    }
}