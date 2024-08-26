using Framework.Core;
using Framework.DependencyInjection;
using Framework.DomainDriven.WebApiNetCore.Auth;
using Framework.SecuritySystem.DependencyInjection;
using Framework.WebApi.Utils.SL;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SampleSystem.BLL;
using SampleSystem.BLL.Core.Jobs;
using SampleSystem.BLL.Jobs;
using SampleSystem.Domain;
using SampleSystem.Events;
using SampleSystem.Generated.DTO;

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
                .AddRelativePaths()
                .RegisterApplicationServices()
                .RegisterJobs();

    private static IServiceCollection AddRelativePaths(this IServiceCollection services) =>
        services.AddRelativeDomainPath((TestExceptObject v) => v.Employee)
                .AddRelativeDomainPath((TestRelativeEmployeeObject v) => v.EmployeeRef1, nameof(TestRelativeEmployeeObject.EmployeeRef1))
                .AddRelativeDomainPath((TestRelativeEmployeeObject v) => v.EmployeeRef2, nameof(TestRelativeEmployeeObject.EmployeeRef2));

    private static IServiceCollection RegisterApplicationServices(this IServiceCollection services) =>
        services.AddScoped<IExampleServiceForRepository, ExampleServiceForRepository>()
                .AddScoped<SampleSystemCustomAribaLocalDBEventMessageSender>()
                .AddSingleton<ISlJsonCompatibilitySerializer, SlJsonCompatibilitySerializer>() // For SL
                .AddKeyedSingleton("DTO", TypeResolverHelper.Create(TypeSource.FromSample<BusinessUnitSimpleDTO>(), TypeSearchMode.Both)) // For legacy audit
                .ReplaceSingleton<IApplicationDefaultUserAuthenticationServiceSettings, EnvironmentDefaultUserAuthenticationServiceSettings>(); // Windows auth

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
