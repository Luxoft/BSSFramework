using Framework.WebApi.Utils;

using Serilog;

namespace SampleSystem.WebApiCore;

public class Program
{
    public static void Main(string[] args)
    {
        CreateWebHostBuilder(args).Build().Run();
        //
        // Log.Logger = Configuration.CreateLoggerBss();
        //
        // try
        // {
        //
        // }
        // catch (Exception ex)
        // {
        //     // Log.Logger.Fatal(ex, "Host terminated unexpectedly");
        // // }
        // finally
        // {
        //     Log.Logger.CloseAndFlush();
        // }
    }

    public static IHostBuilder CreateWebHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilogBss()
            .UseDefaultServiceProvider(
                o =>
                {
                    o.ValidateScopes = true;
                    o.ValidateOnBuild = true;
                })
            .ConfigureWebHostDefaults(
                webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                              .UseConfiguration(Configuration);
                });

    private static IConfiguration Configuration =>
            new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", false, true)
                    .AddEnvironmentVariables()
                    .Build();
}
