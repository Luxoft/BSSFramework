using Anch.Testing.Database.ConnectionStringManagement;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Framework.AutomationCore.ServerManagement.Default;

public class SqlServerFactory(
    IActualTestConnectionStringSource actualTestConnectionStringSource,
    IOptions<AutomationFrameworkSettings> automationFrameworkSettingsOptions) : ISqlServerFactory
{
    private readonly string rootConnectionString =
        new SqlConnectionStringBuilder(actualTestConnectionStringSource.ActualConnectionString.Value) { InitialCatalog = "" }
            .ConnectionString;

    public ISqlServer Create() => new SqlServer(
        new Server(new ServerConnection(new SqlConnection(this.rootConnectionString))),
        automationFrameworkSettingsOptions);
}
