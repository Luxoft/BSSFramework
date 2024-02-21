using Automation.Utils.DatabaseUtils.Interfaces;

using Microsoft.Extensions.Configuration;

namespace Automation.Extensions;

public static class ConfigurationExtensions
{
    public static IConfiguration BuildFromRootWithConnectionStrings(
        this IConfiguration rootConfiguration,
        IDatabaseContext databaseContext) =>
        new ConfigurationBuilder().AddConfiguration(rootConfiguration)
                                  .AddInMemoryCollection(
                                      rootConfiguration.GetSection("ConnectionStrings")
                                                       .GetChildren()
                                                       .ToDictionary(
                                                           x => $"ConnectionStrings:{x.Key}",
                                                           _ => databaseContext.Main.ConnectionString))
                                  .Build();
}
