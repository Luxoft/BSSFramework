using Anch.Core;
using Anch.Testing.Database.ConnectionStringManagement;
using Anch.Testing.Database.DependencyInjection;
using Framework.AutomationCore.ServerManagement;
using Framework.AutomationCore.ServerManagement.Default;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.AutomationCore.Services;

public class BssDatabaseTestingProvider : IDatabaseTestingProvider
{
    public void AddServices(IServiceCollection services) =>
        services.AddSingleton<ITestConnectionStringFactory, BssTestConnectionStringFactory>()
                .AddSingleton<IDatabaseManager, BssDatabaseManager>()
                .AddSingleton<INativeDatabaseManager, NativeDatabaseManager>()
                .AddSingleton<IDatabaseCatalogResolver, DatabaseCatalogResolver>()
                .AddSingleton<ISqlServerFactory, SqlServerFactory>()
                .AddSingleton<IDatabaseFileInfoResolver, DatabaseFileInfoResolver>()
                .AddKeyedSingleton<IInitializer, BssEmptySchemaInitializer>(BssEmptySchemaInitializer.Key)
                .AddSingleton(new ExecuteScriptInfo(@"__Support\Scripts"));
}
