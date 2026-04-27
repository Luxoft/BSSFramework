using Anch.Testing.Database.ConnectionStringManagement;
using Anch.Testing.Database.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace Anch.Testing.Database.Mssql;

public class MssqlDatabaseTestingProvider : IDatabaseTestingProvider
{
    public void AddServices(IServiceCollection services)
    {
        services.AddSingleton<IDatabaseFilePathExtractor, MssqlDatabaseFilePathExtractor>()
            .AddSingleton<ITestDatabaseConnectionStringBuilder, MssqlTestDatabaseConnectionStringBuilder>();
    }
}
