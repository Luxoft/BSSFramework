using Framework.DomainDriven.DBGenerator.Contracts;
using Framework.DomainDriven.Metadata;



namespace Framework.DomainDriven.DBGenerator;

/// <summary>
/// Необходимая информация для генератора скриптов по модификации базы данных
/// </summary>
public class DatabaseScriptGeneratorContext : IDatabaseScriptGeneratorContext
{
    public DatabaseScriptGeneratorContext(DatabaseName databaseName, ISqlDatabaseFactory sqlDatabaseFactory, AssemblyMetadata assemblyMetadata)
    {
        if (databaseName == null)
        {
            throw new ArgumentNullException(nameof(databaseName));
        }

        if (sqlDatabaseFactory == null)
        {
            throw new ArgumentNullException(nameof(sqlDatabaseFactory));
        }

        if (assemblyMetadata == null)
        {
            throw new ArgumentNullException(nameof(assemblyMetadata));
        }

        this.AssemblyMetadata = assemblyMetadata;
        this.SqlDatabaseFactory = sqlDatabaseFactory;
        this.DatabaseName = databaseName;
    }

    /// <summary>
    /// Имя базы данных на которой будет выполнены скрипты
    /// </summary>
    public DatabaseName DatabaseName { get; }

    /// <summary>
    /// Экземпляр Sql сервера на ктором будет выполнены скрипты
    /// </summary>
    public ISqlDatabaseFactory SqlDatabaseFactory { get; }

    /// <summary>
    /// Метаданные доменной модели по которой будут строится скрипты
    /// </summary>
    public AssemblyMetadata AssemblyMetadata { get; }
}
