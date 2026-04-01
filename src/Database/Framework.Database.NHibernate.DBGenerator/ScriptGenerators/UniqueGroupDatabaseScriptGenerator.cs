using CommonFramework;

using Framework.Database.Metadata;
using Framework.Database.NHibernate.DBGenerator.Contracts;
using Framework.Database.NHibernate.DBGenerator.ScriptGenerators.Support;
using Framework.Database.SqlMapper;
using Framework.Restriction;

using Microsoft.SqlServer.Management.Smo;

using Index = Microsoft.SqlServer.Management.Smo.Index;

namespace Framework.Database.NHibernate.DBGenerator.ScriptGenerators;

public class UniqueGroupDatabaseScriptGenerator(params IgnoreLink[] ignore) : PostDatabaseScriptGeneratorBase
{
    private static bool ByName(Index currentIndex, string indexName) => string.Equals(indexName, currentIndex.Name, StringComparison.InvariantCultureIgnoreCase);

    private static bool DifferentColumns(Index index, ICollection<string> columnNames)
    {
        var existIndexColumns = index.IndexedColumns.Select(q => q.Name.ToLowerInvariant()).OrderBy(q => q).ToList();

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
        var result = Array.Empty<(Type, List<string>)>().ToLookup(z => z.Item1, z => z.Item2);
        var metadata = context.AssemblyMetadata;

        if (ignore.Any())
        {
            var typeToMetadataDictionary = metadata.DomainTypes
                                                   .GetAllElements(z => z.NotAbstractChildrenDomainTypes).ToDictionary(z => z.DomainType);

            result = ignore.SelectMany(z =>
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
                                                     return [ValueTuple.Create(z.FromType, new List<string>(0))];
                                                 })
                         .ToLookup(z => z.Item1, z => z.Item2);
        }

        return result;
    }

    private void CreateOrUpdateUniqueIndex(IDatabaseScriptGeneratorContext context, Type declareType, IEnumerable<FieldMetadata> uniqueFields, string indexName)
    {
        var detailTable = context.GetTable(declareType);

        var columnNames = uniqueFields.SelectMany(z => MapperFactory.GetMapping(z).Select(w => w.Name.ToLowerInvariant())).OrderBy(z => z).ToList();

        var index = detailTable.Indexes.FirstOrDefault(currentName => ByName(currentName, indexName));

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
