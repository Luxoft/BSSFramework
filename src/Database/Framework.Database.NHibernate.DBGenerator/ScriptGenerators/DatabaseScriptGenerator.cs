using Anch.Core;

using Framework.Core;
using Framework.Database.Mapping;
using Framework.Database.NHibernate.DBGenerator.Contracts;
using Framework.Database.NHibernate.DBGenerator.ScriptGenerators.ScriptGeneratorStrategy;

namespace Framework.Database.NHibernate.DBGenerator.ScriptGenerators;

/// <summary>
/// Generates a collection of scripts to modify the main database so that it matches the domain model
/// </summary>
public class DatabaseScriptGenerator(
    DatabaseScriptGeneratorMode databaseGeneratorMode = DatabaseScriptGeneratorMode.None,
    string previousPostfix = "_previousVersion",
    ICollection<string>? ignoredIndexes = null,
    IDataTypeComparer? dataTypeComparer = null)
    : IDatabaseScriptGenerator
{
    private readonly IDataTypeComparer dataTypeComparer = dataTypeComparer ?? new DataTypeComparer();

    /// <summary>
    /// Generates an sql script that creates and updates tables and adds or removes columns in those tables, and also creates indexes in those tables
    /// </summary>
    /// <param name="context">SQL server instance and the domain model</param>
    /// <returns>Modification script</returns>
    public IDatabaseScriptResult GenerateScript(IDatabaseScriptGeneratorContext context)
    {
        var metadata = context.AssemblyMetadata;

        var domainTypesLocal = metadata.DomainTypes
                                       .Where(z => !z.DomainType.HasAttribute<ViewAttribute>())
                                       .Where(z => !z.DomainType.HasAttribute<IgnoreMappingAttribute>())
                                       .ToList();

        var databaseScriptGeneratorInfo = new DatabaseScriptGeneratorStrategyInfo(
            context,
            domainTypesLocal,
            databaseGeneratorMode,
            this.dataTypeComparer,
            previousPostfix,
            ignoredIndexes);

        var dictionary = GetScriptGeneratorStrategies(databaseScriptGeneratorInfo)
                             .ToDictionary(
                                 scriptGeneratorStrategy => scriptGeneratorStrategy.ApplyMigrationDbScriptMode,
                                 scriptGeneratorStrategy => scriptGeneratorStrategy.Execute().ToLazy());

        return DatabaseScriptResultFactory.Create(dictionary);
    }

    private static IEnumerable<ScriptGeneratorStrategyBase> GetScriptGeneratorStrategies(DatabaseScriptGeneratorStrategyInfo parameter)
    {
        yield return new AddOrUpdateStrategy(parameter);
        yield return new ChangeIndexesStrategy(parameter);

        if (parameter.DatabaseGeneratorMode.HasFlag(DatabaseScriptGeneratorMode.RemoveObsoleteColumns))
        {
            yield return new RemoveStrategy(parameter);
        }

        yield return new ChangeDefaultInitializedValueStrategy(parameter);
    }
}
