using System;
using System.IO;

using Framework.WebApi.Utils;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

using Serilog;

namespace SampleSystem.WebApiCore;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = Configuration.CreateLoggerBss();

        try
        {
            CreateWebHostBuilder(args).Build().Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseDefaultServiceProvider(o =>
                                              {
                                                  o.ValidateScopes = true;
                                                  o.ValidateOnBuild = true;
                                              })
                   .UseConfiguration(Configuration)
                   .UseSerilogBss()
                   .UseStartup<Startup>();

    private static IConfiguration Configuration =>
            new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", false, true)
                    .AddEnvironmentVariables()
                    .Build();
}
