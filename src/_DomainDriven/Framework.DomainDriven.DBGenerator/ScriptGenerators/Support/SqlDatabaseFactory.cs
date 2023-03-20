using System;
using System.Linq;

using Framework.Core;

using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Framework.DomainDriven.DBGenerator;

/// <summary>
/// Создает объект, который подключен к экземпляру SQL сервера и умеющего создавать базу даных
/// </summary>
public class SqlDatabaseFactory : ISqlDatabaseFactory
{
    private readonly Lazy<Server> _serverLazy;

    public SqlDatabaseFactory(SqlConnectionInfo sqlConnection)
            : this(() => new Server(new ServerConnection(sqlConnection)))
    {
    }

    public SqlDatabaseFactory(Microsoft.Data.SqlClient.SqlConnection sqlConnection)
            : this(() => new Server(new ServerConnection(sqlConnection)))
    {
    }

    private SqlDatabaseFactory(Func<Server> createServerFunc) =>
            this._serverLazy = new Lazy<Server>(createServerFunc);

    /// <summary>
    /// Экземпляр SQL сервера
    /// </summary>
    public Server Server => this._serverLazy.Value;

    /// <summary>
    /// Создает стандартное подключение к экземпляру SQL сервера с именем <param name="serverName"></param>.
    /// Проверка доллинности выполняется с текущими учетными данными Windows
    /// </summary>
    /// <param name="serverName">Имя экземпляра SQL сервера</param>
    /// <returns>Подключенный к SQL серверу объект</returns>
    public static SqlDatabaseFactory CreateDefault(string serverName) =>
            new SqlDatabaseFactory(new SqlConnectionInfo(serverName).Self(z => z.UseIntegratedSecurity = true));

    /// <summary>
    /// Создает стандартное подключение к экземпляру SQL сервера с именем <param name="serverName"></param>.
    /// Проверка доллинности выполняется с переданными учетными данными
    /// </summary>
    /// <param name="serverName">Имя экземпляра SQL сервера</param>
    /// <param name="credentials">Учетная запись используемая для подключения к экземпляру SQL сервера</param>
    /// <returns>Подключенный к SQL серверу объект</returns>
    public static SqlDatabaseFactory Create(string serverName, UserCredential credentials) =>
            new SqlDatabaseFactory(new SqlConnectionInfo(serverName, credentials.UserName, credentials.Password));

    /// <summary>
    /// Возвращает базу данных из экземпляра SQL сервера <see cref="ISqlDatabaseFactory.Server"/> с именем <paramref name="databaseName"/>
    /// </summary>
    /// <param name="databaseName">Имя базы данных</param>
    /// <returns>База данных если она есть, иначе null</returns>
    public Database GetDatabase(DatabaseName databaseName)
    {
        var server = this._serverLazy.Value;

        var result = server.Databases.Cast<Database>().FirstOrDefault(z => z.Name.Equals(databaseName.Name, StringComparison.CurrentCultureIgnoreCase));

        if (result != null)
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
    /// Возвращает базу данных или создает новую если такой нет из экземпляра SQL сервера <see cref="ISqlDatabaseFactory.Server"/> с именем <paramref name="databaseName"/>
    /// </summary>
    /// <param name="databaseName">Имя базы данных</param>
    /// <returns>База данных</returns>
    public Database GetOrCreateDatabase(DatabaseName databaseName)
    {
        var server = this._serverLazy.Value;

        var result = server.Databases.Cast<Database>().FirstOrDefault(z => z.Name.Equals(databaseName.Name, StringComparison.CurrentCultureIgnoreCase));
        if (null == result)
        {
            result = new Database(server, databaseName.Name);
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

    private static void TryCreateSchema(Database result, string schemaName)
    {
        if (!result.Schemas.OfType<Schema>().Any(z => string.Equals(z.Name, schemaName, StringComparison.InvariantCultureIgnoreCase)))
        {
            var schema = new Schema(result, schemaName);
            schema.Create();
        }
    }
}
