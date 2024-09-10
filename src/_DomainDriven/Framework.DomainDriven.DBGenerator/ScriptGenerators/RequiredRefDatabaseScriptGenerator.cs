using Framework.Core;
using Framework.DomainDriven.DAL.Sql;
using Framework.DomainDriven.DBGenerator.Contracts;
using Framework.DomainDriven.DBGenerator.ScriptGenerators;
using Framework.DomainDriven.Metadata;
using Framework.Restriction;

using Microsoft.SqlServer.Management.Smo;

using NHibernate.Util;

using Index = Microsoft.SqlServer.Management.Smo.Index;

namespace Framework.DomainDriven.DBGenerator;

public class RequiredRefDatabaseScriptGenerator : PostDatabaseScriptGeneratorBase
{
    private readonly HashSet<IgnoreLink> _ignoreDomainTypeLinksHash;

    public RequiredRefDatabaseScriptGenerator(IEnumerable<IgnoreLink> ignoreDomainTypeLinks)
    {
        if (ignoreDomainTypeLinks == null) throw new ArgumentNullException(nameof(ignoreDomainTypeLinks));

        this._ignoreDomainTypeLinksHash = ignoreDomainTypeLinks.ToHashSet();
    }

    protected override void Apply(IDatabaseScriptGeneratorContext context)
    {
        context.SqlDatabaseFactory.Server.ConnectionContext.CapturedSql.Add("------------------------------Generate RequiredRef-----------------------");

        var requiredRefContext = new RequiredRefContext(context);

        this.ProcessRequeredAttribute(requiredRefContext);
        this.ProcessMasterRef(requiredRefContext);
        this.ProcessViews(requiredRefContext);

        var createIndexActions = requiredRefContext.RecreateIndexies.Select(z =>
                                                                            {
                                                                                var index = new Index(z.Parent, z.Name);
                                                                                index.IndexKeyType = z.IndexKeyType;
                                                                                index.IsUnique = z.IsUnique;
                                                                                index.IsClustered = z.IsClustered;

                                                                                z.IndexedColumns
                                                                                 .Cast<IndexedColumn>()
                                                                                 .Foreach(q => index.IndexedColumns.Add(new IndexedColumn(index, q.Name, q.Descending)));

                                                                                z.Drop();

                                                                                return index;
                                                                            }).ToList();

        var removableViews = this.GetRemovableViews(requiredRefContext);

        var recreateViewInfos = removableViews
                                .Select(z => new { Database = z.Parent, Name = z.Name, TextHeader = z.TextHeader, TextBody = z.TextBody, TextMode = z.TextMode, Schema = z.Schema })
                                .ToList();

        removableViews.Foreach(z => z.Drop());

        requiredRefContext.RequeredColumns.Foreach(z => z.Nullable = false);

        requiredRefContext.RequeredColumns.Foreach(z => z.Alter());

        createIndexActions.Foreach(z => z.Create());

        recreateViewInfos.Reverse();

        recreateViewInfos.Foreach(
                                  z =>
                                  {
                                      var newView = new View(z.Database, z.Name, z.Schema)
                                                    {
                                                            TextHeader = z.TextHeader,
                                                            TextBody = z.TextBody,
                                                            TextMode = z.TextMode
                                                    };

                                      newView.Create();
                                  });

        context.SqlDatabaseFactory.Server.ConnectionContext.CapturedSql.Add("------------------------------End RequiredRef-----------------------");
    }

    private IList<View> GetRemovableViews(RequiredRefContext refContext)
    {
        var viewToDeepDictionary = new Dictionary<View, Lazy<int>>();

        var walker = new DependencyWalker(refContext.Context.SqlDatabaseFactory.Server);

        foreach (var view in refContext.RecreateViews)
        {
            this.InitViewDeeps(view, viewToDeepDictionary, walker);
        }

        return viewToDeepDictionary.OrderBy(z => z.Value.Value).Select(z => z.Key).ToList();
    }

    private void InitViewDeeps(View view, Dictionary<View, Lazy<int>> viewDictionary, DependencyWalker walker)
    {
        Lazy<int> storageRemoveOrder;

        if (viewDictionary.TryGetValue(view, out storageRemoveOrder))
        {
            return;
        }

        var tree = walker.DiscoverDependencies(new[] { view }, false);
        var result = walker.WalkDependencies(tree);
        var filteredResult = result.Where(z => z.Urn.Type == "View").ToList();
        var nextViews = filteredResult.Select(z => walker.Server.GetSmoObject(z.Urn)).Cast<View>().Where(z => z != view).ToList();

        if (nextViews.Any())
        {
            viewDictionary.Add(view, new Lazy<int>(() => nextViews.Select(z => viewDictionary[z]).Max(z => z.Value) + 1));
        }
        else
        {
            viewDictionary.Add(view, new Lazy<int>(() => 0));
        }

        foreach (var nextView in nextViews)
        {
            this.InitViewDeeps(nextView, viewDictionary, walker);
        }
    }

    private void ProcessViews(RequiredRefContext refContext)
    {
        foreach (var databaseGrouped in refContext.RequeredColumns.GroupBy(z => ((Database)((Table)z.Parent).Parent)))
        {
            var database = (Database)databaseGrouped.Key;

            if (!database.Views.Any())
            {
                continue;
            }

            var affectedTables = databaseGrouped.Select(z => z.Parent).Cast<Table>().ToHashSet();

            var walker = new DependencyWalker(refContext.Context.SqlDatabaseFactory.Server);

            foreach (var view in database.Views.Cast<View>().Where(z => !z.IsSystemObject))
            {
                var tree = walker.DiscoverDependencies(new[] { view }, true);

                var result = walker.WalkDependencies(tree);

                var filteredResult = result.Where(z => z.Urn.Type != "View").ToList();

                var dependendiesTables = filteredResult.Select(z =>
                                                               {
                                                                   try
                                                                   {
                                                                       return database.Parent.GetSmoObject(z.Urn);
                                                                   }
                                                                   catch (Exception e)
                                                                   {
                                                                       return null;
                                                                   }
                                                               })
                                                       .Where(z => null != z)
                                                       .ToHashSet();

                if (dependendiesTables.Any(z => affectedTables.Contains(z)))
                {
                    refContext.Add(view);
                }
            }
        }
    }

    private void ProcessRequeredAttribute(RequiredRefContext refContext)
    {
        var context = refContext.Context;

        var metadata = context.AssemblyMetadata;

        var domainTypeWithRequiredFields = metadata
                                           .DomainTypes.GetAllElements(z => z.NotAbstractChildrenDomainTypes)
                                           .Select(z =>
                                                           new
                                                           {
                                                                   DomainTypeMetadata = z,
                                                                   RequeredFields = z.Fields.Where(q => !(q is ListTypeFieldMetadata))
                                                                                     .Where(q => q.Attributes.HasAttribute<RequiredAttribute>()).ToList()
                                                           })
                                           .SelectMany(z => z.RequeredFields.Select(q => new { DomainTypeMetadata = z.DomainTypeMetadata, RequeredField = q }))
                                           .Where(z => !this._ignoreDomainTypeLinksHash
                                                            .Any(link => link.FromType.IsAssignableFrom(z.DomainTypeMetadata.DomainType)
                                                                         && link.MemberInfo.PropertyType == z.RequeredField.Type))
                                           .ToList();

        foreach (var domainTypeWithRequiredField in domainTypeWithRequiredFields.Where(z => !z.DomainTypeMetadata.IsView))
        {
            var table = context.GetTable(domainTypeWithRequiredField.DomainTypeMetadata.DomainType);

            var columnNames = MapperFactory.GetMapping(domainTypeWithRequiredField.RequeredField).Select(z => z.Name).ToHashSet();

            this.ApplyRequered(refContext, table, columnNames);
        }
    }

    private void ProcessMasterRef(RequiredRefContext refContext)
    {
        var context = refContext.Context;

        var metadata = context.AssemblyMetadata;

        var typeWithDetails = metadata
                              .DomainTypes
                              .Where(z => z.ListFields.Any(q => metadata.PersistentDomainObjectBaseType.IsAssignableFrom(q.ElementType)))
                              .SelectMany(
                                          z => z.ListFields
                                                .Where(q => metadata.PersistentDomainObjectBaseType.IsAssignableFrom(q.ElementType))
                                                .Where(q => q.ElementType != z.DomainType) //ignore hierarh
                                                .Select(q => new { DomainType = z, ListField = q }))
                              .Where(z => !this._ignoreDomainTypeLinksHash.Any(q => q.FromType.IsAssignableFrom(z.ListField.ElementType) && q.MemberInfo.PropertyType == z.DomainType.DomainType))
                              .ToList();

        if (!typeWithDetails.Any())
        {
            return;
        }

        var dictionary = metadata.DomainTypes.GetAllElements(z => z.NotAbstractChildrenDomainTypes).ToDictionary(z => z.DomainType);

        foreach (var typeWithDetail in typeWithDetails)
        {
            var masterMetadata = typeWithDetail.DomainType;

            var detailMetadata = this.GetDomainTypeMetadataBy(typeWithDetail.ListField.ElementType, dictionary);

            if (detailMetadata.IsView)
            {
                continue;
            }

            var table = context.GetTable(detailMetadata.DomainType);

            var masterRef = GetMasterRefMetadata(detailMetadata, masterMetadata);

            var masterRefColumnName = MapperFactory.GetMapping(masterRef).First().Name;

            this.ApplyRequered(refContext, table, masterRefColumnName);
        }
    }

    private void ApplyRequered(RequiredRefContext context, Table table, string columnName)
    {
        this.ApplyRequered(context, table, new string[] { columnName });
    }

    private void ApplyRequered(RequiredRefContext refContext, Table table, IEnumerable<string> columnNames)
    {
        var columnNamesHashSet = columnNames.Select(z => z.ToLower()).ToHashSet();

        var columns = table.Columns.Cast<Column>().Where(z => columnNamesHashSet.Contains(z.Name.ToLower())).ToList();

        if (columns.All(z => !z.Nullable))
        {
            return;
        }

        var indexed = table.Indexes.Cast<Index>()
                           .Where(z => z.IndexedColumns.Cast<IndexedColumn>().Any(q => columnNamesHashSet.Contains(q.Name.ToLower()))).ToList();

        refContext.Add(indexed);
        refContext.Add(columns);
    }

    private DomainTypeMetadata GetDomainTypeMetadataBy(Type type, Dictionary<Type, DomainTypeMetadata> dictionary)
    {
        DomainTypeMetadata result = null;
        var currentType = type;
        while (currentType != typeof(object))
        {
            if (dictionary.TryGetValue(currentType, out result))
            {
                break;
            }

            currentType = currentType.BaseType;
        }

        if (null == result)
        {
            throw new ArgumentException("No domainTypeMetadata for {0} type", type.Name);
        }

        return result;
    }

    private class RequiredRefContext
    {
        private readonly HashSet<Index> recreateIndexies;
        private readonly List<Column> requeredColumns;
        private readonly IDatabaseScriptGeneratorContext context;
        private readonly HashSet<View> expectedRemovableViews;

        public RequiredRefContext(IDatabaseScriptGeneratorContext contex)
        {
            this.context = contex;
            this.recreateIndexies = new HashSet<Index>();
            this.requeredColumns = new List<Column>();
            this.expectedRemovableViews = new HashSet<View>();
        }

        public IDatabaseScriptGeneratorContext Context
        {
            get { return this.context; }
        }

        public IEnumerable<Index> RecreateIndexies
        {
            get { return this.recreateIndexies; }
        }

        public IEnumerable<Column> RequeredColumns
        {
            get { return this.requeredColumns; }
        }

        public IEnumerable<View> RecreateViews
        {
            get { return this.expectedRemovableViews; }
        }

        public void Add(IEnumerable<Index> indexed)
        {
            this.recreateIndexies.AddRange(indexed);
        }

        public void Add(IEnumerable<Column> columns)
        {
            this.requeredColumns.AddRange(columns);
        }

        public void Add(IEnumerable<View> views)
        {
            this.expectedRemovableViews.AddRange(views);
        }

        public void Add(View view)
        {
            this.Add(new[] { view });
        }
    }
}
