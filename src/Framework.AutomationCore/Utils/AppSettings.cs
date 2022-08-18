using System;
using System.IO;

using Microsoft.Extensions.Configuration;

namespace Automation.Utils;

public static class AppSettings
{
    public static IConfiguration Default { get; private set; }

    public static void Initialize(string environmentVariablesPrefix = null)
    {
        Default = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($@"appsettings.json", false)
            .AddJsonFile($@"{Environment.MachineName}.appsettings.json", true)
            .AddEnvironmentVariables(environmentVariablesPrefix)
            .Build();
    }
}