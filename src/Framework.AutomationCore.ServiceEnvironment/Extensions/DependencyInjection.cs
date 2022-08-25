using Automation.ServiceEnvironment.Services;
using Framework.Core.Services;
using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate.Audit;
using Framework.DomainDriven.ServiceModel.IAD;
using Microsoft.Extensions.DependencyInjection;

namespace Automation.ServiceEnvironment;

public static class DependencyInjection
{
    public static IServiceCollection AddTestAuthentication(
        this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IntegrationTestUserAuthenticationService>();
        serviceCollection.AddSingletonFrom<IAuditRevisionUserAuthenticationService, IntegrationTestUserAuthenticationService>();
        serviceCollection.AddSingletonFrom<IDefaultUserAuthenticationService, IntegrationTestUserAuthenticationService>();

        return serviceCollection;
    }

    public static IServiceCollection AddTestDateTimeService(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IntegrationTestDateTimeService>();
        serviceCollection.ReplaceSingletonFrom<IDateTimeService, IntegrationTestDateTimeService>();

        return serviceCollection;
    }
}