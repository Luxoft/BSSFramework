using Framework.Database.Metadata;
using Framework.Database.NHibernate.DBGenerator.Contracts;

namespace Framework.Database.NHibernate.DBGenerator.ScriptGenerators.Support;

/// <summary>
/// Необходимая информация для генератора скриптов по модификации базы данных
/// </summary>
public class DatabaseScriptGeneratorContext(DatabaseName databaseName, ISqlDatabaseFactory sqlDatabaseFactory, AssemblyMetadata assemblyMetadata)
    : IDatabaseScriptGeneratorContext
{
    /// <summary>
    /// Имя базы данных на которой будет выполнены скрипты
    /// </summary>
    public DatabaseName DatabaseName { get; } = databaseName ?? throw new ArgumentNullException(nameof(databaseName));

    /// <summary>
    /// Экземпляр Sql сервера на ктором будет выполнены скрипты
    /// </summary>
    public ISqlDatabaseFactory SqlDatabaseFactory { get; } = sqlDatabaseFactory ?? throw new ArgumentNullException(nameof(sqlDatabaseFactory));

    /// <summary>
    /// Метаданные доменной модели по которой будут строится скрипты
    /// </summary>
    public AssemblyMetadata AssemblyMetadata { get; } = assemblyMetadata ?? throw new ArgumentNullException(nameof(assemblyMetadata));
}

