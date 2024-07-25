using Framework.DomainDriven.DBGenerator.Contracts;

namespace Framework.DomainDriven.DBGenerator;

public interface IMainDBScriptGeneratorBuilder : IConfigurable
{
    /// <summary>
    /// Setups generator with selected <paramref name="mode"/>
    /// </summary>
    /// <param name="mode">Generation mode</param>
    /// <param name="previousColumnPostfix">Postfix to add to removed or changed columns</param>
    /// <param name="ignoredIndexes">List of index name that would be not generated</param>
    /// <param name="dataTypeComparer">Custom data type comparer</param>
    /// <returns></returns>
    IMainDBScriptGeneratorBuilder WithMain(
        DatabaseScriptGeneratorMode mode = DatabaseScriptGeneratorMode.AutoGenerateUpdateChangeTypeScript,
        string previousColumnPostfix = "_previousVersion",
        ICollection<string> ignoredIndexes = null,
        IDataTypeComparer dataTypeComparer = null);

    IMainDBScriptGeneratorBuilder WithUniqueGroup(params IgnoreLink[] ignore);

    IMainDBScriptGeneratorBuilder WithCustom(IDatabaseScriptGenerator service);

    IMainDBScriptGeneratorBuilder WithRequireRef(params IgnoreLink[] ignoreLinks);

    /// <summary>
    /// Настройка, предотвращающая удаление временной БД, предназначенной для накатывания схемы.
    /// </summary>
    /// <returns><see cref="IMainDBScriptGeneratorBuilder"/>.</returns>
    IMainDBScriptGeneratorBuilder WithPreserveSchemaDatabase();

    IMigrationScriptGeneratorBuilder MigrationBuilder { get; }
}
