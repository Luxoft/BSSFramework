using Framework.DomainDriven.NHibernate;
using Microsoft.SqlServer.Management.Smo;

namespace Framework.DomainDriven.DBGenerator
{
    /// <summary>
    /// Создает объект, который подключается к экземпляру SQL сервера и умеет создавать базу даных
    /// </summary>
    public interface ISqlDatabaseFactory
    {
        /// <summary>
        /// Экземпляр SQL сервера
        /// </summary>
        Server Server { get; }

        /// <summary>
        /// Возвращает базу данных из экземпляра SQL сервера <see cref="Server"/> с именем <paramref name="databaseName"/>
        /// </summary>
        /// <param name="databaseName">Имя базы данных</param>
        /// <returns>База данных</returns>
        Database GetDatabase(DatabaseName databaseName);

        /// <summary>
        /// Возвращает базу данных или создает новую если такой нет из экземпляра SQL сервера <see cref="Server"/> с именем <paramref name="databaseName"/>
        /// </summary>
        /// <param name="databaseName">Имя базы данных</param>
        /// <param name="schemaName">Имя схемы</param>
        /// <returns>База данных</returns>
        Database GetOrCreateDatabase(DatabaseName databaseName);
    }
}
