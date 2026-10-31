using Framework.Core;

using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Framework.Database.NHibernate.DBGenerator.ScriptGenerators.Support;

/// <summary>
/// Creates an object that is connected to an SQL server instance and can create a database
/// </summary>
public class SqlDatabaseFactory : ISqlDatabaseFactory
{
    private readonly Lazy<Server> serverLazy;

    public SqlDatabaseFactory(SqlConnectionInfo sqlConnection)
            : this(() => new Server(new ServerConnection(sqlConnection)))
    {
    }

    public SqlDatabaseFactory(Microsoft.Data.SqlClient.SqlConnection sqlConnection)
            : this(() => new Server(new ServerConnection(sqlConnection)))
    {
    }

    private SqlDatabaseFactory(Func<Server> createServerFunc) =>
            this.serverLazy = new Lazy<Server>(createServerFunc);

    /// <summary>
    /// SQL server instance
    /// </summary>
    public Server Server => this.serverLazy.Value;

    /// <summary>
    /// Creates a standard connection to the SQL server instance named <paramref name="serverName"/>.
    /// The validity check is performed with the current Windows credentials
    /// </summary>
    /// <param name="serverName">SQL server instance name</param>
    /// <returns>Object connected to the SQL server</returns>
    public static SqlDatabaseFactory CreateDefault(string serverName) =>
            new(new SqlConnectionInfo(serverName).Self(z => z.UseIntegratedSecurity = true));

    /// <summary>
    /// Creates a standard connection to the SQL server instance named <paramref name="serverName"/>.
    /// The validity check is performed with the provided credentials
    /// </summary>
    /// <param name="serverName">SQL server instance name</param>
    /// <param name="credentials">Account used to connect to the SQL server instance</param>
    /// <returns>Object connected to the SQL server</returns>
    public static SqlDatabaseFactory Create(string serverName, DbUserCredential credentials) =>
            new(new SqlConnectionInfo(serverName, credentials.UserName, credentials.Password));

    /// <summary>
    /// Returns a database from the SQL server instance <see cref="ISqlDatabaseFactory.Server"/> with the name <paramref name="databaseName"/>
    /// </summary>
    /// <param name="databaseName">Database name</param>
    /// <returns>The database if it exists, otherwise null</returns>
    public Microsoft.SqlServer.Management.Smo.Database? GetDatabase(DatabaseName databaseName)
    {
        var server = this.serverLazy.Value;

        var result = server.Databases.FirstOrDefault(z => z.Name.Equals(databaseName.Name, StringComparison.CurrentCultureIgnoreCase));

        if (result is not null)
        {
            if (result.Schemas
                      .OfType<Schema>()
                      .Any(z => string.Equals(z.Name, databaseName.Schema, StringComparison.InvariantCultureIgnoreCase)))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        return result;
    }

    /// <summary>
    /// Returns a database or creates a new one if none exists from the SQL server instance <see cref="ISqlDatabaseFactory.Server"/> with the name <paramref name="databaseName"/>
    /// </summary>
    /// <param name="databaseName">Database name</param>
    /// <returns>Database</returns>
    public Microsoft.SqlServer.Management.Smo.Database GetOrCreateDatabase(DatabaseName databaseName)
    {
        var server = this.serverLazy.Value;

        var result = server.Databases.FirstOrDefault(z => z.Name.Equals(databaseName.Name, StringComparison.CurrentCultureIgnoreCase));
        if (null == result)
        {
            result = new Microsoft.SqlServer.Management.Smo.Database(server, databaseName.Name);
            result.Create();
        }

        TryCreateSchema(result, databaseName.Schema);

        var auditDatabaseName = databaseName as AuditDatabaseName;

        if (null != auditDatabaseName)
        {
            TryCreateSchema(result, auditDatabaseName.RevisionEntitySchema);
        }

        return result;
    }

    private static void TryCreateSchema(Microsoft.SqlServer.Management.Smo.Database result, string schemaName)
    {
        if (!result.Schemas.OfType<Schema>().Any(z => string.Equals(z.Name, schemaName, StringComparison.InvariantCultureIgnoreCase)))
        {
            var schema = new Schema(result, schemaName);
            schema.Create();
        }
    }
}
