using Framework.Core;
using Framework.DomainDriven.DAL.Sql;
using Framework.DomainDriven.DBGenerator.Contracts;
using Framework.DomainDriven.DBGenerator.ScriptGenerators;
using Framework.DomainDriven.Metadata;
using Framework.Restriction;
using Microsoft.SqlServer.Management.Smo;

using Index = Microsoft.SqlServer.Management.Smo.Index;

namespace Framework.DomainDriven.DBGenerator;

public class UniqueGroupDatabaseScriptGenerator : PostDatabaseScriptGeneratorBase
{
    private readonly IgnoreLink[] ignoreLinks;

    public UniqueGroupDatabaseScriptGenerator(params IgnoreLink[] ignore)
    {
        this.ignoreLinks = ignore;
    }

    private static bool ByName(Index currentIndex, string indexName)
    {
        return string.Equals(indexName, currentIndex.Name, StringComparison.InvariantCultureIgnoreCase);
    }

    private static bool DifferentColumns(Index index, ICollection<string> columnNames)
    {
        var existIndexColumns = index.IndexedColumns.Cast<IndexedColumn>().Select(q => q.Name.ToLowerInvariant()).OrderBy(q => q).ToList();

        return !columnNames.SequenceEqual(existIndexColumns);
    }

    private static void CreateIndex(Table detailTable, string indexName, List<string> columnNames)
    {
        var index = new Index(detailTable, indexName);
        columnNames.Foreach(z => index.IndexedColumns.Add(new IndexedColumn(index, z)));

        index.IndexKeyType = IndexKeyType.DriUniqueKey;

        detailTable.Indexes.Add(index);

        index.Create();
    }

    protected override void Apply(IDatabaseScriptGeneratorContext context)
    {
        context.SqlDatabaseFactory.Server.ConnectionContext.CapturedSql.Add("------------------------------Generate Unique-----------------------");

        var metadata = context.AssemblyMetadata;
        var ignoreLookup = this.GetIgnoreLinks(context);

        var info = metadata.DomainTypes.SelectMany(z => new[] { z }.Concat(z.NotAbstractChildrenDomainTypes))
                           .SelectMany(z => z.UniqueIndexes.Where(q => q.Fields.All(w => w.DomainTypeMetadata == z)).Select(uniqueIndexMetadata => new { DomainMetadata = z, UniqueIndexMetadata = uniqueIndexMetadata }))
                           .Where(z =>
                                          !ignoreLookup.Contains(z.DomainMetadata.DomainType)
                                          || (ignoreLookup[z.DomainMetadata.DomainType].Any(q => q.Any()) && ignoreLookup[z.DomainMetadata.DomainType].All(q => !q.Intersect(z.UniqueIndexMetadata.Fields.Select(e => e.Name)).Any())))
                           .ToList();

        info.Foreach(z => this.CreateOrUpdateUniqueIndex(context, z.DomainMetadata.DomainType, z.UniqueIndexMetadata.Fields.ToList(), z.UniqueIndexMetadata.FriendlyName));

        context.SqlDatabaseFactory.Server.ConnectionContext.CapturedSql.Add("------------------------------Generate End Unique-----------------------");
    }

    private ILookup<Type, List<string>> GetIgnoreLinks(IDatabaseScriptGeneratorContext context)
    {
        var result = new ValueTuple<Type, List<string>>[0].ToLookup(z => z.Item1, z => z.Item2);
        var metadata = context.AssemblyMetadata;

        if (this.ignoreLinks.Any())
        {
            var typeToMetadataDictionary = metadata.DomainTypes
                                                   .GetAllElements(z => z.NotAbstractChildrenDomainTypes).ToDictionary(z => z.DomainType);

            result = this.ignoreLinks.SelectMany(z =>
                                                 {
                                                     if (z.MemberInfo != null)
                                                     {
                                                         var targetDomainTypeMetadata = typeToMetadataDictionary[z.FromType];
                                                         var propertyMetadata =
                                                                 targetDomainTypeMetadata.ListFields.First(
                                                                  q =>
                                                                          string.Equals(q.Name, z.MemberInfo.Name, StringComparison.InvariantCultureIgnoreCase));

                                                         var propertyMetadataUniqueGroupAttribute =
                                                                 propertyMetadata.Attributes.OfType<UniqueGroupAttribute>().FirstOrDefault();
                                                         if (propertyMetadataUniqueGroupAttribute == null)
                                                         {
                                                             throw new ArgumentException(
                                                              $"Property:{propertyMetadata.Name} not mark {typeof(UniqueGroupAttribute).Name} attribute");
                                                         }

                                                         var propertyTypeMetadata = typeToMetadataDictionary[propertyMetadata.ElementType];

                                                         // Уникальные индексы, построенные на основе ссылки на свойстве коллекции
                                                         var uniqueIndexesFromRef =
                                                                 new UniqueIndexMetadataReader(propertyTypeMetadata.AssemblyMetadata).ReadFromReferenceTo(
                                                                  propertyTypeMetadata);

                                                         return
                                                                 uniqueIndexesFromRef.Select(
                                                                  w =>
                                                                          ValueTuple.Create(propertyMetadata.ElementType, w.Fields.Select(e => e.Name).ToList()));
                                                     }
                                                     return new[] { ValueTuple.Create(z.FromType, new List<string>(0)) };
                                                 })
                         .ToLookup(z => z.Item1, z => z.Item2);
        }

        return result;
    }

    private void CreateOrUpdateUniqueIndex(IDatabaseScriptGeneratorContext context, Type declareType, IEnumerable<FieldMetadata> uniqueFields, string indexName)
    {
        var detailTable = context.GetTable(declareType);

        var columnNames = uniqueFields.SelectMany(z => MapperFactory.GetMapping(z).Select(w => w.Name.ToLowerInvariant())).OrderBy(z => z).ToList();

        var index = detailTable.Indexes.Cast<Index>().FirstOrDefault(currentName => ByName(currentName, indexName));

        if (null == index)
        {
            CreateIndex(detailTable, indexName, columnNames);
        }
        else if (index.IndexKeyType != IndexKeyType.DriUniqueKey || !index.IsUnique || DifferentColumns(index, columnNames))
        {
            index.Drop();

            CreateIndex(detailTable, indexName, columnNames);
        }
    }
}
