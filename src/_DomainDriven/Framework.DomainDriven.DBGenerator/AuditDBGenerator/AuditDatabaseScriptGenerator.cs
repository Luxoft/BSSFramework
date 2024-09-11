using System.Collections.ObjectModel;
using System.Reflection;

using Framework.Core;
using Framework.DomainDriven.DAL.Revisions;
using Framework.DomainDriven.DBGenerator;
using Framework.DomainDriven.DBGenerator.Contracts;
using Framework.DomainDriven.DBGenerator.Team;
using Framework.DomainDriven.NHibernate.Audit;
using Framework.Persistent.Mapping;
using Framework.Projection;

using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Dialect.Schema;
using NHibernate.Envers.Configuration.Attributes;
using NHibernate.Id;
using NHibernate.Mapping;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Util;

using Environment = NHibernate.Cfg.Environment;

namespace Framework.DomainDriven.NHibernate;

public class AuditDatabaseScriptGenerator : IDatabaseScriptGenerator
{
    private readonly IReadOnlyList<IMappingSettings> mappingSettings;

    private readonly string auditTablePostFix;

    public AuditDatabaseScriptGenerator(IEnumerable<IMappingSettings> mappingSettings,
                                        string auditTablePostfix)
    {
        if (mappingSettings == null) throw new ArgumentNullException(nameof(mappingSettings));

        this.auditTablePostFix = auditTablePostfix;

        this.mappingSettings = mappingSettings.ToList();
    }

    public string AuditTablePostFix => this.auditTablePostFix;

    public IDatabaseScriptResult GenerateScript(IDatabaseScriptGeneratorContext context)
    {

        var initial = this.GetScripts(context).ToList();

        var dictionary = new Dictionary<ApplyMigrationDbScriptMode, Lazy<IEnumerable<string>>>
                         {
                                 {
                                         ApplyMigrationDbScriptMode.AddOrUpdate,
                                         new Lazy<IEnumerable<string>>(() => initial)
                                 }
                         };

        return DatabaseScriptResultFactory.Create(dictionary);
    }

    private IEnumerable<string> GetScripts(IDatabaseScriptGeneratorContext context)
    {
        var databaseName = this.mappingSettings.Single(z => z.IsAudited()).AuditDatabase;

        var generateContext = this.GetGenerateContext(context);

        var stmt = generateContext.Connection.CreateCommand();

        if (generateContext.Configuration.Properties.TryGetValue("command_timeout", out var commandTimeoutUnparsed)
            && int.TryParse(commandTimeoutUnparsed, out var commandTimeout))
        {
            stmt.CommandTimeout = commandTimeout;
        }

        try
        {
            var scripts = this.GetScripts(generateContext);

            foreach (var scriptItem in scripts)
            {
                stmt.CommandText = scriptItem;
                stmt.ExecuteNonQuery();
            }

            return scripts;
        }

        finally
        {
            if (stmt != null)
            {
                stmt.Dispose();
            }

            generateContext.AuditConnectionHelper.Release();
            generateContext.OriginalConnectionHelper.Release();
        }
    }

    private AuditGenerateContext GetGenerateContext(IDatabaseScriptGeneratorContext context)
    {
        var cfg = this.CreateConfiguration(context.DatabaseName.Name, context);

        var auditClassMappings = cfg.ClassMappings
                                    .Where(z => z.MappedClass == null || typeof(AuditRevisionEntity).IsAssignableFrom(z.MappedClass))
                                    .ToList();

        var dialect = Dialect.GetDialect(cfg.Properties);

        cfg.BuildMappings();

        var mapping = cfg.BuildMapping();

        IConnectionHelper auditConnectionHelper = new ManagedProviderConnectionHelper(cfg.Properties);

        auditConnectionHelper.Prepare();

        var auditConnection = auditConnectionHelper.Connection;

        var extendedSchema = new MsSqlDataBaseSchemaExtended(auditConnectionHelper.Connection);

        var auditDatabaseMetadata = new DatabaseMetadata(auditConnection, dialect);


        var schemaField = typeof(DatabaseMetadata).GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                                                  .Single(
                                                          z => z.FieldType == typeof(IDataBaseSchema),
                                                          () => throw new ArgumentException($"Expected private field of type :'{nameof(IDataBaseSchema)}'"));


        schemaField.SetValue(auditDatabaseMetadata, extendedSchema);


        var defaultCatalog = PropertiesHelper.GetString(Environment.DefaultCatalog, cfg.Properties, null);

        var generateContext = new AuditGenerateContext(auditClassMappings, auditDatabaseMetadata, dialect, mapping, defaultCatalog, cfg, context.DatabaseName.Schema, this.mappingSettings, context);
        return generateContext;
    }

    private IList<string> GetScripts(AuditGenerateContext generateContext)
    {
        var sqlScript = new List<string>();
        var alsoCreatedTable = new HashSet<string>();
        var auditClassMappings = generateContext.PersistentClasses;
        var auditDatabaseMetadata = generateContext.AuditDatabaseMetadata;
        var originalDatabaseMetadata = generateContext.OriginalDatabaseMetadata;
        var defaultCatalog = generateContext.DefaultCatalog;
        var dialect = generateContext.Dialect;
        var mapping = generateContext.Mapping;
        var cfg = generateContext.Configuration;

        var tablePostFix = "Audit";

        var auditAttributeService = generateContext.MappingSettings.GetAuditAttributeService(cfg.ClassMappings);

        var auditClass2OriginalClassItems = cfg.ClassMappings.Where(m => m.MappedClass == null || !m.MappedClass.IsProjection())
                                               .Join(
                                                     auditClassMappings,
                                                     z => z.Table.Name.ToLower().Replace("[", string.Empty).Replace("]", string.Empty),
                                                     z => z.Table.Name.Replace("[", string.Empty)
                                                           .Replace("]", string.Empty)
                                                           .Replace(tablePostFix, string.Empty)
                                                           .ToLower(),
                                                     (original, audit) => new { audit, original });

        var auditClass2OriginalClassDict = new Dictionary<PersistentClass, PersistentClass>();

        foreach (var item in auditClass2OriginalClassItems)
        {
            // Skip not audited domain objects
            if (Attribute.GetCustomAttribute(item.original.MappedClass, typeof(NotAuditedClassAttribute)) != null)
            {
                continue;
            }

            if (auditClass2OriginalClassDict.ContainsKey(item.audit))
            {
                var duplicate1 = auditClass2OriginalClassDict[item.audit].EntityName;
                var duplicate2 = item.original.EntityName;
                throw new NotSupportedException($"You cannot have two or more domain objects ({duplicate1}, {duplicate2}) that reference to the same DB table and both to be Audited. Use NotAuditedClass to mark not audited domain object");
            }

            auditClass2OriginalClassDict.Add(item.audit, item.original);
        }

        foreach (var classMapping in auditClassMappings)
        {
            PersistentClass original;
            if (auditClass2OriginalClassDict.TryGetValue(classMapping, out original)) // auditEntityEntry
            {
                if (auditAttributeService.GetAttributeFor(original.MappedClass) == RelationTargetAuditMode.NotAudited)
                {
                    continue;
                }
            }

            var table = classMapping.Table;

            var tableMetadata = auditDatabaseMetadata.GetTableMetadata(table.Name, generateContext.AuditScheme, null, table.IsQuoted);

            var expectedOriginalTable = table.Name.SkipLast(this.AuditTablePostFix, StringComparison.InvariantCultureIgnoreCase);

            var originalTableMetadata = originalDatabaseMetadata.GetTableMetadata(expectedOriginalTable, generateContext.AuditScheme, null, table.IsQuoted);

            var context = new AuditTableGenerateContext(originalTableMetadata, tableMetadata, table, dialect, mapping, defaultCatalog, null, original, auditAttributeService, generateContext.Context);

            var isCreateMode = tableMetadata == null && !alsoCreatedTable.Contains(table.Name);

            var createOrAlterTableScripts = CreateCreateOrAlterTableScripts(context, isCreateMode);

            if (isCreateMode && string.Equals(classMapping.EntityName, typeof(AuditRevisionEntity).FullName, StringComparison.InvariantCultureIgnoreCase))
            {
                createOrAlterTableScripts = new[] { $"IF NOT EXISTS (  SELECT [name] FROM sys.tables WHERE [name] = '{table.Name}' ) {createOrAlterTableScripts.First()}" };
            }

            sqlScript.AddRange(createOrAlterTableScripts);

            if (table.IsPhysicalTable && table.SchemaActions.HasFlag(SchemaAction.Update))
            {
                sqlScript.AddRange(CreateForeignKey(context));
                sqlScript.AddRange(CreateIndexScript(context));
            }

            sqlScript.AddRange(this.GetIterateScripts(auditDatabaseMetadata, cfg, dialect));

            alsoCreatedTable.Add(table.Name);
        }

        return sqlScript;
    }

    private Configuration CreateConfiguration(string auditDbName, IDatabaseScriptGeneratorContext context)
    {
        var cfg = new Configuration();

        var connectionInitializer = new NHibConnectionInitializer(context.SqlDatabaseFactory.Server.ConnectionContext.ConnectionString, auditDbName);

        connectionInitializer.Initialize(cfg);

        var overrideMappingSettings = this.mappingSettings.Select(z => new MappingSettingsWrapper(z, auditDbName)).ToList();

        if (!overrideMappingSettings.Any())
        {
            throw new ArgumentException("Framework.DomainDriven.Settings no has empty assemblyWithMapping");
        }

        foreach (var ms in overrideMappingSettings)
        {
            ms.Initializer.Initialize(cfg);
        }

        cfg.InitializeAudit(overrideMappingSettings, LazyInterfaceImplementHelper.CreateNotImplemented<IAuditRevisionUserAuthenticationService>());

        SchemaMetadataUpdater.QuoteTableAndColumns(cfg, Dialect.GetDialect(cfg.Properties));

        return cfg;
    }

    private class MappingSettingsWrapper : IMappingSettings
    {
        private readonly IMappingSettings source;

        private readonly string overrideAuditDatabaseName;

        public MappingSettingsWrapper(IMappingSettings source, string overrideAuditDatabaseName)
        {
            this.source = source;
            this.overrideAuditDatabaseName = overrideAuditDatabaseName;
        }

        public Type PersistentDomainObjectBaseType => this.source.PersistentDomainObjectBaseType;

        public DatabaseName Database => this.source.Database;

        public AuditDatabaseName AuditDatabase => new AuditDatabaseName(this.overrideAuditDatabaseName, this.source.AuditDatabase.Schema, this.source.AuditDatabase.RevisionEntitySchema);

        public ReadOnlyCollection<Type> Types => this.source.Types;

        public IConfigurationInitializer Initializer => this.source.Initializer;

        public IAuditTypeFilter GetAuditTypeFilter()
        {
            return this.source.GetAuditTypeFilter();
        }
    }
    private static IEnumerable<string> CreateForeignKey(AuditTableGenerateContext context)
    {
        if (context.Dialect.SupportsForeignKeyConstraintInAlterTable)
        {
            var fqn = context.Table.GetQualifiedName(context.Dialect).Replace("[", string.Empty).Replace("]", string.Empty);

            foreach (var fk in context.Table.ForeignKeyIterator)
            {
                if (fk.HasPhysicalConstraint &&
                    fk.ReferencedTable.SchemaActions.HasFlag(SchemaAction.Update))
                {
                    var database = context.Context.SqlDatabaseFactory.GetDatabase(context.Context.DatabaseName);
                    database.Tables.Refresh();

                    var table = database.Tables.Cast<Microsoft.SqlServer.Management.Smo.Table>()
                                        .FirstOrDefault(z => string.Equals($"{database.Name}.{z.Schema}.{z.Name}", fqn, StringComparison.InvariantCultureIgnoreCase) ||
                                                             string.Equals($"{z.Schema}.{z.Name}", fqn, StringComparison.InvariantCultureIgnoreCase));

                    bool create = true;

                    if (table != null)
                    {

                        var expectedReferenceColumns =
                                fk.ReferencedColumns.Select(z => z.Name)
                                  .IfEmpty(() => fk.ReferencedTable.PrimaryKey.Columns.Select(z => z.Name))
                                  .Select(z => z.TrimStart('[').TrimEnd(']'))
                                  .Select(z => z.ToLower())
                                  .OrderBy(z => z)
                                  .ToList();

                        foreach (
                                var actualForeignKey in
                                table.ForeignKeys.Cast<Microsoft.SqlServer.Management.Smo.ForeignKey>())
                        {
                            if (string.Equals(actualForeignKey.ReferencedTable, fk.ReferencedTable.GetQuotedName(),
                                              StringComparison.InvariantCultureIgnoreCase))
                            {
                                var actualReferenceColumns =
                                        actualForeignKey.Columns
                                                        .Cast<Microsoft.SqlServer.Management.Smo.ForeignKeyColumn>()
                                                        .Select(z => z.ReferencedColumn)
                                                        .Select(z => z.ToLower())
                                                        .OrderBy(z => z)
                                                        .ToList();

                                if (expectedReferenceColumns.SequenceEqual(actualReferenceColumns))
                                {
                                    create = false;
                                    break;
                                }
                            }
                        }
                    }

                    if (create)
                    {
                        yield return fk.SqlCreateString(context.Dialect, context.Mapping, context.DefaultCatalog, null);
                    }
                }
            }
        }
    }

    private static IEnumerable<string> CreateIndexScript(AuditTableGenerateContext context)
    {
        return
                context.Table.IndexIterator.Where(z => z.Table == null || context.TableInfo.GetIndexMetadata(z.Name) == null).Select(
                 z => z.SqlCreateString(context.Dialect, context.Mapping, context.DefaultCatalog, null));
    }

    private IEnumerable<string> GetIterateScripts(DatabaseMetadata databaseMetadata, Configuration cfg, Dialect dialect)
    {
        foreach (var generator in this.IterateGenerators(cfg, dialect))
        {
            string key = generator.GeneratorKey();
            if (!databaseMetadata.IsSequence(key) && !databaseMetadata.IsTable(key))
            {
                return generator.SqlCreateStrings(dialect);
            }
        }
        return new string[0];
    }

    private static IEnumerable<string> CreateCreateOrAlterTableScripts(AuditTableGenerateContext context, bool createMode)
    {
        if (createMode)
        {
            return new AuditSqlSriptGenerator().GenerateCreateScripts(context);
        }

        return new AuditSqlSriptGenerator().GenerateUpdateScripts(context);
    }


    private IEnumerable<IPersistentIdentifierGenerator> IterateGenerators(Configuration configuration, Dialect dialect)
    {
        var generators = new Dictionary<string, IPersistentIdentifierGenerator>();
        var properties = configuration.Properties;
        string defaultCatalog = PropertiesHelper.GetString(Environment.DefaultCatalog, properties, null);
        string defaultSchema = PropertiesHelper.GetString(Environment.DefaultSchema, properties, null);

        foreach (var pc in configuration.ClassMappings)
        {
            if (!pc.IsInherited)
            {
                var ig =
                        pc.Identifier.CreateIdentifierGenerator(dialect, defaultCatalog, defaultSchema, (RootClass)pc) as
                                IPersistentIdentifierGenerator;

                if (ig != null)
                {
                    generators[ig.GeneratorKey()] = ig;
                }
            }
        }

        foreach (var collection in configuration.CollectionMappings)
        {
            if (collection.IsIdentified)
            {
                var ig =
                        ((IdentifierCollection)collection).Identifier.CreateIdentifierGenerator(dialect, defaultCatalog, defaultSchema,
                            null) as IPersistentIdentifierGenerator;

                if (ig != null)
                {
                    generators[ig.GeneratorKey()] = ig;
                }
            }
        }

        return generators.Values;
    }
}
