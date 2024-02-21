using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serilog;
using Serilog.Core;

namespace Framework.WebApi.Utils;

public static class LoggingExtensions
{
    /// <summary>
    /// Turns on Serilog as web project logger.
    /// Configures Serilog if not configured yet
    /// </summary>
    /// <param name="builder">A builder for Microsoft.AspNetCore.Hosting.IWebHost</param>
    public static IHostBuilder UseSerilogBss(this IHostBuilder builder) =>
            builder
                    .ConfigureLogging((unused, z) => z.ClearProviders().AddSerilog(dispose: true))
                    .UseSerilog();

    /// <summary>
    /// Configures Serilog
    /// </summary>
    /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
    /// <param name="tags">Additional tags that will be sent to InfluxDb</param>
    public static Logger CreateLoggerBss(
            this IConfiguration configuration,
            Dictionary<string, string> tags = null) =>
            new LoggerConfiguration().CreateLogger(configuration, tags);

    private static Logger CreateLogger(
            this LoggerConfiguration configuration,
            IConfiguration hostConfiguration,
            Dictionary<string, string> tags)
    {
        var logger = configuration.ReadFrom.Configuration(hostConfiguration);

        foreach (var (key, value) in hostConfiguration.GetGlobalTags(tags))
        {
            logger = logger.Enrich.WithProperty(key, value);
        }

        return logger.CreateLogger();
    }
}
