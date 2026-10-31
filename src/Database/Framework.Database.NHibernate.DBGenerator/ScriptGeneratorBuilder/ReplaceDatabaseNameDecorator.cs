using Framework.Database.NHibernate.DBGenerator.Contracts;
using Framework.Database.NHibernate.DBGenerator.ScriptGenerators.Support;

namespace Framework.Database.NHibernate.DBGenerator.ScriptGeneratorBuilder;

/// <summary>
/// Generates a collection of scripts to modify the main database with a changed name so that it matches the domain model
/// </summary>
internal class ReplaceDatabaseNameDecorator(Func<IDatabaseScriptGeneratorContext, DatabaseName> nextDatabaseNameFunc, IDatabaseScriptGenerator source)
    : IDatabaseScriptGenerator
{
    private readonly IDatabaseScriptGenerator source = source ?? throw new ArgumentNullException(nameof(source));
    private readonly Func<IDatabaseScriptGeneratorContext, DatabaseName> nextDatabaseNameFunc = nextDatabaseNameFunc ?? throw new ArgumentNullException(nameof(nextDatabaseNameFunc));

    /// <summary>
    /// Generates an sql script that creates and updates tables and adds or removes columns in those tables, and also creates indexes in those tables
    /// </summary>
    /// <param name="context">SQL server instance and the domain model</param>
    /// <returns>Modification script</returns>
    public IDatabaseScriptResult GenerateScript(IDatabaseScriptGeneratorContext context) => this.source.GenerateScript(new DatabaseScriptGeneratorContext(this.nextDatabaseNameFunc(context), context.SqlDatabaseFactory, context.AssemblyMetadata));
}
