using Framework.SecuritySystem;

using Hangfire;
using Hangfire.Dashboard;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.HangfireCore;

public static class HangfireExtensions
{
    /// <summary>
    /// переключает регулярные джобы с шины на Hangfire.
    /// (см. http://readthedocs/docs/iad-framework/en/master/KB/webApiUtils/webApiUtilsHangfire.html)
    /// </summary>
    public static IServiceCollection AddHangfireBss(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IBssHangfireSettings> setup)
    {
        var settings = new BssHangfireSettings();

        configuration.GetSection("Hangfire").Bind(settings);

        setup(settings);

        settings.Initialize(services, configuration);

        return services;
    }

    public static IApplicationBuilder UseHangfireBss(
        this IApplicationBuilder app,
        string dashboardUrl = "/admin/jobs",
        IDashboardAuthorizationFilter? authorizationFilter = null,
        DomainSecurityRule.RoleBaseSecurityRule? dashboardAuthorizationSecurityRule = null)
    {
        var settings = app.ApplicationServices.GetRequiredService<BssHangfireSettings>();

        if (settings.Enabled)
        {
            app.UseHangfireDashboard(
                dashboardUrl,
                new DashboardOptions
                {
                    DashboardTitle = "Regular jobs",
                    Authorization =
                    [
                        authorizationFilter ?? new AdminHangfireAuthorization(dashboardAuthorizationSecurityRule ?? SecurityRole.Administrator)
                    ]
                });

            settings.RunJobs();
        }

        return app;
    }
}
