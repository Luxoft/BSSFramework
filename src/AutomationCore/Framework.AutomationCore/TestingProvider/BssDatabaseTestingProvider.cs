using Anch.Testing.Database.ConnectionStringManagement;
using Anch.Testing.Database.DependencyInjection;

using Framework.AutomationCore.Services.DatabaseUtils;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.AutomationCore.TestingProvider;

public class BssDatabaseTestingProvider : IDatabaseTestingProvider
{
    public void AddServices(IServiceCollection services) =>
        services.AddSingleton<ITestConnectionStringFactory, BssTestConnectionStringFactory>()
                .AddSingleton<IDatabaseManager, BssDatabaseManager>()
                .AddSingleton<INativeDatabaseManager, NativeDatabaseManager>()
                .AddSingleton<IDatabaseCatalogResolver, DatabaseCatalogResolver>()

                .AddSingleton<IDatabaseFileInfoResolver, DatabaseFileInfoResolver>()
                .AddSingleton<IDatabaseContext, DatabaseContext>();
}
