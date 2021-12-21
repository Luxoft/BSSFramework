using System;

using Framework.HangfireCore;

using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;

using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.DependencyInjection
{
    public static class HangfireExtensions
    {
        /// <summary>
        /// переключает регулярные джобы с шины на Hangfire.
        /// (см. http://readthedocs/docs/iad-framework/en/master/KB/webApiUtils/webApiUtilsHangfire.html)
        /// </summary>
        public static IServiceCollection AddHangfireBss(
            this IServiceCollection services,
            IConfiguration configuration,
            SqlServerStorageOptions storageOptions = null)
        {
            var settings = new HangfireSettings();
            configuration.GetSection("Hangfire").Bind(settings);

            services.AddHangfire(
                z =>
                {
                    z
                        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                        .UseSimpleAssemblyNameTypeSerializer()
                        .UseRecommendedSerializerSettings()
                        .UseSerilogLogProvider()
                        .UseSqlServerStorage(
                            () => new SqlConnection(settings.ConnectionString),
                            storageOptions ?? new SqlServerStorageOptions
                            {
                                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                                QueuePollInterval = TimeSpan.Zero,
                                UseRecommendedIsolationLevel = true,
                                UsePageLocksOnDequeue = true,
                                DisableGlobalLocks = true
                            });
                });

            services.AddHangfireServer();

            return services;
        }

        /// <summary>
        /// переключает регулярные джобы с шины на Hangfire.
        /// (см. http://readthedocs/docs/iad-framework/en/master/KB/webApiUtils/webApiUtilsHangfire.html)
        /// </summary>
        public static IApplicationBuilder UseHangfireBss(
           this IApplicationBuilder app,
           IConfiguration configuration,
           Action<JobTiming[]> runJobsFunc,
           string dashboardUrl = "/admin/jobs",
           IDashboardAuthorizationFilter authorizationFilter = null)
        {
            app.UseHangfireDashboard(
                dashboardUrl,
                new DashboardOptions
                {
                    DashboardTitle = "Regular jobs",
                    Authorization = new[] { authorizationFilter ?? new BaseHangfireAuthorization() }
                });

            var settings = new HangfireSettings();
            configuration.GetSection("Hangfire").Bind(settings);

            runJobsFunc(settings.JobTimings);

            return app;
        }
    }
}
