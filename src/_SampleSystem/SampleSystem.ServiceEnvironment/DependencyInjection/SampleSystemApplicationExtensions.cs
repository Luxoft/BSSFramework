using Framework.DependencyInjection;
using Framework.SecuritySystem;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SampleSystem.BLL;
using SampleSystem.BLL.Core.Jobs;
using SampleSystem.BLL.Jobs;
using SampleSystem.Domain;
using SampleSystem.Security.Services;

namespace SampleSystem.ServiceEnvironment;

public static class SampleSystemApplicationExtensions
{
    public static IServiceCollection RegisterGeneralApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration) =>
        services.AddHttpContextAccessor()
                .AddLogging()
                .AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<EmployeeBLL>())
                .RegisterSmtpNotification(configuration)
                .RegisterApplicationServices()
                .RegisterJobs();

    private static IServiceCollection RegisterApplicationServices(this IServiceCollection services) =>
        services.AddScoped<IExampleServiceForRepository, ExampleServiceForRepository>()

                .AddScoped(typeof(CurrentEmployeeSecurityProvider<>))
                .AddSingleton<IRelativeDomainPathInfo<Employee, Employee>>(
                    new RelativeDomainPathInfo<Employee, Employee>(employee => employee))

                .AddSingleton(typeof(TestRestrictionObjectConditionFactory<>))
                .AddSingleton<IRelativeDomainPathInfo<TestRestrictionObject, TestRestrictionObject>>(
                    new RelativeDomainPathInfo<TestRestrictionObject, TestRestrictionObject>(v => v));

    private static IServiceCollection RegisterSmtpNotification(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterNotificationJob();
        services.RegisterNotificationSmtp(configuration);
        services.RegisterRewriteReceiversDependencies(configuration);

        return services;
    }

    private static IServiceCollection RegisterJobs(this IServiceCollection services) =>
        services.AddScoped<ISampleJob, SampleJob>();
}
