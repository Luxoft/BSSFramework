using System;

using Automation.ServiceEnvironment.Services;

using Framework.Core.Services;
using Framework.DependencyInjection;
using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate.Audit;
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

            .AddSingleton<IntegrationTestUserAuthenticationService>()
            .ReplaceSingletonFrom<IAuditRevisionUserAuthenticationService, IntegrationTestUserAuthenticationService>()
            .ReplaceSingletonFrom<IDefaultUserAuthenticationService, IntegrationTestUserAuthenticationService>()

            .AddSingleton<IntegrationTestDateTimeService>()
            .ReplaceSingletonFrom<IDateTimeService, IntegrationTestDateTimeService>()

            .AddScoped<TestWebApiCurrentMethodResolver>()
            .ReplaceScopedFrom<IWebApiCurrentMethodResolver, TestWebApiCurrentMethodResolver>()

            .ReplaceSingleton<IWebApiExceptionExpander, TestWebApiExceptionExpander>();

        return serviceCollection
               .ValidateDuplicateDeclaration()
               .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });
    }
}
