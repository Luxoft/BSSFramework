using Framework.DomainDriven.Metadata;

namespace Framework.DomainDriven.DBGenerator.Contracts;

/// <summary>
/// Необходымая информация для генератора скриптов по модификации базы данных
/// </summary>
public interface IDatabaseScriptGeneratorContext
{
    /// <summary>
    /// Имя базы данных на которой будет выполнены скрипты
    /// </summary>
    DatabaseName DatabaseName { get; }

    /// <summary>
    /// Экземпляр Sql сервера на ктором будет выполнены скрипты
    /// </summary>
    ISqlDatabaseFactory SqlDatabaseFactory { get; }

    /// <summary>
    /// Метаданные доменной модели по которой будут строиться скрипты
    /// </summary>
    AssemblyMetadata AssemblyMetadata { get; }
}
