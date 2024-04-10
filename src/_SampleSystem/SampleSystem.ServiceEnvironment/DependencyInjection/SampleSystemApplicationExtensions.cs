using Framework.Authorization.BLL;
using Framework.Cap;
using Framework.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SampleSystem.BLL;
using SampleSystem.BLL.Core.Jobs;
using SampleSystem.BLL.Jobs;

namespace SampleSystem.ServiceEnvironment;

public static class SampleSystemApplicationExtensions
{
    public static IServiceCollection RegisterGeneralApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
    {
        return services.AddHttpContextAccessor()
                       .AddLogging()
                       .AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<EmployeeBLL>())
                       .RegisterSmtpNotification(configuration)
                       .RegisterApplicationServices()
                       .AddCapBss(configuration.GetConnectionString("DefaultConnection"))
                       .RegisterJobs();
    }

    private static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IExampleServiceForRepository, ExampleServiceForRepository>();

        services.ReplaceScoped<IAuthorizationValidator, SampleSystemCustomAuthValidator>();

        return services;
    }

    private static IServiceCollection RegisterSmtpNotification(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterNotificationJob();
        services.RegisterNotificationSmtp(configuration);
        services.RegisterRewriteReceiversDependencies(configuration);

        return services;
    }

    private static IServiceCollection RegisterJobs(this IServiceCollection services)
    {
        services.AddScoped<ISampleJob, SampleJob>();

        return services;
    }
}
