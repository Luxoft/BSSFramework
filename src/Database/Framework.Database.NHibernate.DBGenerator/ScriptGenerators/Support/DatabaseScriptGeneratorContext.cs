using Framework.Database.Metadata;
using Framework.Database.NHibernate.DBGenerator.Contracts;

namespace Framework.Database.NHibernate.DBGenerator.ScriptGenerators.Support;

/// <summary>
/// Information required by the database modification script generator
/// </summary>
public class DatabaseScriptGeneratorContext(DatabaseName databaseName, ISqlDatabaseFactory sqlDatabaseFactory, AssemblyMetadata assemblyMetadata)
    : IDatabaseScriptGeneratorContext
{
    /// <summary>
    /// Name of the database on which the scripts will be executed
    /// </summary>
    public DatabaseName DatabaseName { get; } = databaseName ?? throw new ArgumentNullException(nameof(databaseName));

    /// <summary>
    /// SQL server instance on which the scripts will be executed
    /// </summary>
    public ISqlDatabaseFactory SqlDatabaseFactory { get; } = sqlDatabaseFactory ?? throw new ArgumentNullException(nameof(sqlDatabaseFactory));

    /// <summary>
    /// Metadata of the domain model from which scripts will be built
    /// </summary>
    public AssemblyMetadata AssemblyMetadata { get; } = assemblyMetadata ?? throw new ArgumentNullException(nameof(assemblyMetadata));
}
