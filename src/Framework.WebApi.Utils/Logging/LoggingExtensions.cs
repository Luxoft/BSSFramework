using Microsoft.Extensions.Hosting;

using Serilog;

namespace Framework.WebApi.Utils;

public static class LoggingExtensions
{
    public static IHostBuilder UseSerilogBss(this IHostBuilder builder, Dictionary<string, string> tags = null) =>
        builder
            .UseSerilog(
                (context, services, configuration) =>
                {
                    configuration
                        .ReadFrom.Configuration(context.Configuration)
                        .ReadFrom.Services(services);

                    foreach (var (key, value) in context.Configuration.GetGlobalTags(tags)) configuration.Enrich.WithProperty(key, value);
                },
                true);
}
