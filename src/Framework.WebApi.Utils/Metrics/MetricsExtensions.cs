using App.Metrics;
using App.Metrics.Formatters.Json;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.WebApi.Utils;

public static class MetricsExtensions
{
    /// <summary>
    /// Turns on metrics reporting to Influx DB.
    /// Requires "InfluxDb" configuration section with following properties: Uri, Database.
    /// Takes additional configuration properties from "GlobalTags" section if any are provided.
    /// </summary>
    /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
    /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
    /// <param name="satisfactoryResponseTime">Satisfying web response time threshold in seconds</param>
    /// <param name="tags">Additional tags that will be sent to InfluxDb</param>
    public static IServiceCollection AddMetricsBss(
            this IServiceCollection services,
            IConfiguration configuration,
            double? satisfactoryResponseTime = null,
            Dictionary<string, string> tags = null)
    {
        var influxDb = new InfluxDbSettings();
        configuration.GetSection("InfluxDb").Bind(influxDb);

        var metrics = AppMetrics.CreateDefaultBuilder()
                                .Configuration.Configure(
                                                         z =>
                                                         {
                                                             foreach (var (key, value) in configuration.GetGlobalTags(tags))
                                                             {
                                                                 z.GlobalTags.Add(key, value);
                                                             }
                                                         })
                                .Report.ToInfluxDb(influxDb.Uri, influxDb.Database)
                                .Build();

        services
                .AddMetrics(metrics)
                .AddMetricsEndpoints(configuration, options => options.MetricsEndpointOutputFormatter = new MetricsJsonOutputFormatter())
                .AddMetricsTrackingMiddleware(
                                              z =>
                                              {
                                                  if (!satisfactoryResponseTime.HasValue)
                                                  {
                                                      return;
                                                  }

                                                  z.ApdexTrackingEnabled = true;
                                                  z.ApdexTSeconds = satisfactoryResponseTime.Value;
                                              })
                .AddMetricsReportingHostedService();

        return services;
    }
}
