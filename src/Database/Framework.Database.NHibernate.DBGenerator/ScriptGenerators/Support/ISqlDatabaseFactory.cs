using Microsoft.SqlServer.Management.Smo;

namespace Framework.Database.NHibernate.DBGenerator.ScriptGenerators.Support;

/// <summary>
/// Creates an object that connects to an SQL server instance and can create a database
/// </summary>
public interface ISqlDatabaseFactory
{
    /// <summary>
    /// SQL server instance
    /// </summary>
    Server Server { get; }

    /// <summary>
    /// Returns a database from the SQL server instance <see cref="Server"/> with the name <paramref name="databaseName"/>
    /// </summary>
    /// <param name="databaseName">Database name</param>
    /// <returns>Database</returns>
    Microsoft.SqlServer.Management.Smo.Database? GetDatabase(DatabaseName databaseName);

    /// <summary>
    /// Returns a database or creates a new one if none exists from the SQL server instance <see cref="Server"/> with the name <paramref name="databaseName"/>
    /// </summary>
    /// <param name="databaseName">Database name</param>
    /// <returns>Database</returns>
    Microsoft.SqlServer.Management.Smo.Database GetOrCreateDatabase(DatabaseName databaseName);
}
