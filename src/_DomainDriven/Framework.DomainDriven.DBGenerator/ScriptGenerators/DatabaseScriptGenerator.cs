using CommonFramework;

using Framework.Core;
using Framework.DomainDriven.DBGenerator.Contracts;
using Framework.DomainDriven.DBGenerator.ScriptGenerators.ScriptGeneratorStrategy;
using Framework.Persistent.Mapping;

namespace Framework.DomainDriven.DBGenerator;

/// <summary>
/// Генерирует коллекцию скриптов для модификации основной базы данных, таких, чтобы она соответсвовала доменной медели
/// </summary>
public class DatabaseScriptGenerator(
    DatabaseScriptGeneratorMode databaseGeneratorMode = DatabaseScriptGeneratorMode.None,
    string previusPostfix = "_previousVersion",
    ICollection<string> ignoredIndexes = null,
    IDataTypeComparer dataTypeComparer = null)
    : IDatabaseScriptGenerator
{
    private readonly IDataTypeComparer _dataTypeComparer = dataTypeComparer ?? new DataTypeComparer();

    /// <summary>
    /// Генерирует sql скрипт, который создает и обновляет таблицы и добавляет или удаляет колонки в этих таблицах, а так же создает индексы в этих таблицах
    /// </summary>
    /// <param name="context">Экземпляр sql сервера и доменная модель</param>
    /// <returns>Скрипт модификации</returns>
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
            this._dataTypeComparer,
            previusPostfix,
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
