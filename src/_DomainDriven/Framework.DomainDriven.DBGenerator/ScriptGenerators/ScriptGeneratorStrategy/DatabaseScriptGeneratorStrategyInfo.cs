using Framework.DomainDriven.DBGenerator.Contracts;
using Framework.DomainDriven.Metadata;
using Microsoft.SqlServer.Management.Smo;

namespace Framework.DomainDriven.DBGenerator.ScriptGenerators.ScriptGeneratorStrategy;

/// <summary>
/// Предоставляет доступ к параметрам, необходимым для генерации основной базы данных, и хранит списки добавленных и удаленных колонок
/// </summary>
public struct DatabaseScriptGeneratorStrategyInfo
{
    public readonly IEnumerable<DomainTypeMetadata> DomainTypesLocal;

    public readonly IDatabaseScriptGeneratorContext Context;

    public readonly DatabaseScriptGeneratorMode DatabaseGeneratorMode;

    public readonly DataTypeComparer DataTypeComparer;

    public readonly string PreviusPostfix;

    public readonly IList<Tuple<Table, Column, string>> AddedColumns;

    public readonly IList<Column> RemovableColumns;

    public readonly IReadOnlyDictionary<Type, DomainTypeMetadata> TypeToDomainTypeMetadataDictionary;

    public readonly ICollection<string> IgnoredIndexes;

    public DatabaseScriptGeneratorStrategyInfo(
            IDatabaseScriptGeneratorContext context,
            IEnumerable<DomainTypeMetadata> domainTypesLocal,
            DatabaseScriptGeneratorMode databaseGeneratorMode,
            DataTypeComparer dataTypeComparer,
            string previusPostfix,
            ICollection<string> ignoredIndexes)
    {
        this.DomainTypesLocal = domainTypesLocal;
        this.Context = context;
        this.DatabaseGeneratorMode = databaseGeneratorMode;
        this.DataTypeComparer = dataTypeComparer;
        this.PreviusPostfix = previusPostfix;

        this.AddedColumns = new List<Tuple<Table, Column, string>>();
        this.RemovableColumns = new List<Column>();

        this.IgnoredIndexes = ignoredIndexes ?? new List<string>();

        this.TypeToDomainTypeMetadataDictionary = context.AssemblyMetadata.DomainTypes.ToDictionary(z => z.DomainType, z => z);
    }
}
