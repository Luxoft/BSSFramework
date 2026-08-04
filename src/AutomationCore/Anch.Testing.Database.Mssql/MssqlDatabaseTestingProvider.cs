using Anch.Core;
using Anch.Testing.Database.ConnectionStringManagement;
using Anch.Testing.Database.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace Anch.Testing.Database.Mssql;

public class MssqlDatabaseTestingProvider : IDatabaseTestingProvider
{
    private readonly MssqlDatabaseSettings? settings;

    public MssqlDatabaseTestingProvider()
    {
    }

    public MssqlDatabaseTestingProvider(MssqlDatabaseSettings settings) => this.settings = settings;

    public void AddServices(IServiceCollection services)
    {
        if (this.settings is not null)
        {
            services.AddSingleton(this.settings);
        }

        services
            .AddSingleton<ITestConnectionStringFactory, MssqlTestConnectionStringFactory>()
            .AddSingleton<IDatabaseManager, MssqlDatabaseManager>()
            .AddSingleton<INativeDatabaseManager, NativeDatabaseManager>()
            .AddSingleton<IDatabaseCatalogResolver, DatabaseCatalogResolver>()
            .AddSingleton<ISqlServerFactory, MssqlServerFactory>()
            .AddSingleton<IDatabaseFileInfoResolver, DatabaseFileInfoResolver>()
            .AddKeyedSingleton<IInitializer, MssqlEmptySchemaInitializer>(MssqlEmptySchemaInitializer.Key);
    }
}
