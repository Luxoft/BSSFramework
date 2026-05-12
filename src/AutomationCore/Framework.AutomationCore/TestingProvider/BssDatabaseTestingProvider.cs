using Anch.Testing.Database.ConnectionStringManagement;
using Anch.Testing.Database.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.AutomationCore.TestingProvider;

public class BssDatabaseTestingProvider : IDatabaseTestingProvider
{
    public void AddServices(IServiceCollection services) =>
        services.AddSingleton<IDatabaseFilePathExtractor, BssDatabaseFilePathExtractor>()
                .AddSingleton<ITestDatabaseConnectionStringBuilder, BssTestDatabaseConnectionStringBuilder>()
                .AddSingleton<IDatabaseManager, BssDatabaseManager>()

                .AddSingleton<IMsSqlServerSource, MsSqlServerSource>();
}
