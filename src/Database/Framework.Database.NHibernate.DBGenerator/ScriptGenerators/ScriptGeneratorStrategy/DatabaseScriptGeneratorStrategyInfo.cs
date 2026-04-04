using Framework.Database.Metadata;
using Framework.Database.NHibernate.DBGenerator.Contracts;

using Microsoft.SqlServer.Management.Smo;

namespace Framework.Database.NHibernate.DBGenerator.ScriptGenerators.ScriptGeneratorStrategy;

/// <summary>
/// Предоставляет доступ к параметрам, необходимым для генерации основной базы данных, и хранит списки добавленных и удаленных колонок
/// </summary>
public struct DatabaseScriptGeneratorStrategyInfo
{
    public readonly IEnumerable<DomainTypeMetadata> DomainTypesLocal;

    public readonly IDatabaseScriptGeneratorContext Context;

    public readonly DatabaseScriptGeneratorMode DatabaseGeneratorMode;

    public readonly IDataTypeComparer DataTypeComparer;

    public readonly string PreviousPostfix;

    public readonly List<Tuple<Table, Column, string>> AddedColumns;

    public readonly List<Column> RemovableColumns;

    public readonly IReadOnlyDictionary<Type, DomainTypeMetadata> TypeToDomainTypeMetadataDictionary;

    public readonly ICollection<string> IgnoredIndexes;

    public DatabaseScriptGeneratorStrategyInfo(
        IDatabaseScriptGeneratorContext context,
        IEnumerable<DomainTypeMetadata> domainTypesLocal,
        DatabaseScriptGeneratorMode databaseGeneratorMode,
        IDataTypeComparer dataTypeComparer,
        string previousPostfix,
        ICollection<string>? ignoredIndexes)
    {
        this.DomainTypesLocal = domainTypesLocal;
        this.Context = context;
        this.DatabaseGeneratorMode = databaseGeneratorMode;
        this.DataTypeComparer = dataTypeComparer;
        this.PreviousPostfix = previousPostfix;

        this.AddedColumns = [];
        this.RemovableColumns = [];

        this.IgnoredIndexes = ignoredIndexes ?? new List<string>();

        this.TypeToDomainTypeMetadataDictionary = context.AssemblyMetadata.DomainTypes.ToDictionary(z => z.DomainType, z => z);
    }
}
