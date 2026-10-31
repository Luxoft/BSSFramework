using Framework.Database.Metadata;
using Framework.Database.NHibernate.DBGenerator.ScriptGenerators.Support;

namespace Framework.Database.NHibernate.DBGenerator.Contracts;

/// <summary>
/// Information required by the database modification script generator
/// </summary>
public interface IDatabaseScriptGeneratorContext
{
    /// <summary>
    /// Name of the database on which the scripts will be executed
    /// </summary>
    DatabaseName DatabaseName { get; }

    /// <summary>
    /// SQL server instance on which the scripts will be executed
    /// </summary>
    ISqlDatabaseFactory SqlDatabaseFactory { get; }

    /// <summary>
    /// Metadata of the domain model from which scripts will be built
    /// </summary>
    AssemblyMetadata AssemblyMetadata { get; }
}
