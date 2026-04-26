using Anch.Core;

using Framework.Database.Metadata;
using Framework.Database.NHibernate.DBGenerator.Contracts;
using Framework.Database.NHibernate.DBGenerator.ScriptGenerators.Support;
using Framework.Database.NHibernate.DBGenerator.Team;

using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Framework.Database.NHibernate.DBGenerator.ScriptGenerators;

public class TryCreateDatabaseScriptGenerator(IDatabaseScriptGenerator source) : IDatabaseScriptGenerator
{
    public IDatabaseScriptResult GenerateScript(IDatabaseScriptGeneratorContext context)
    {
        IDatabaseScriptResult tryCreateDatabase = null;

        var prevMode = context.SqlDatabaseFactory.Server.ConnectionContext.SqlExecutionModes;

        context.SqlDatabaseFactory.Server.ConnectionContext.SqlExecutionModes = SqlExecutionModes.ExecuteAndCaptureSql;

        context.SqlDatabaseFactory.Server.ConnectionContext.CapturedSql.Clear();

        context.SqlDatabaseFactory.GetOrCreateDatabase(context.DatabaseName);

        var tryCreateScript = context.SqlDatabaseFactory.Server.ConnectionContext.CapturedSql.GetScriptsForBatchExecuting();

        context.SqlDatabaseFactory.Server.ConnectionContext.SqlExecutionModes = prevMode;

        var tryCreateDictionary = new Dictionary<ApplyMigrationDbScriptMode, Lazy<IEnumerable<string>>>();

        tryCreateDictionary.Add(ApplyMigrationDbScriptMode.PreAddOrUpdate, new Lazy<IEnumerable<string>>(() => tryCreateScript));

        tryCreateDatabase = DatabaseScriptResultFactory.Create(tryCreateDictionary);

        var result = source.GenerateScript(context);

        return new[] { tryCreateDatabase, result }.Combine();
    }
}
/// <summary>
/// Копирует схему базу данных и на ней выполняет скрипты
/// </summary>
public class UnsafeApplyChangedDatabaseScriptGenerator : IDatabaseScriptGenerator
{
    private readonly IDatabaseScriptGenerator source;
    private readonly bool copyData;
    private readonly string[] copyDataForTables;

    private readonly bool removeSchemaDatabase;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnsafeApplyChangedDatabaseScriptGenerator"/> class.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="copyData">if set to <c>true</c> [copy data].</param>
    /// <param name="removeSchemaDatabase">if set to <c>true</c> [remove schema database].</param>
    /// <param name="copyDataForTables">The copy data for tables.</param>
    public UnsafeApplyChangedDatabaseScriptGenerator(
            IDatabaseScriptGenerator source,
            bool copyData,
            bool removeSchemaDatabase,
            string[] copyDataForTables = null)
    {
        this.source = source;
        this.copyData = copyData;
        this.copyDataForTables = copyDataForTables;
        this.removeSchemaDatabase = removeSchemaDatabase;
    }

    public IDatabaseScriptResult GenerateScript(IDatabaseScriptGeneratorContext context)
    {
        var contextWrapper = new DatabaseScriptGeneratorContextWrapper(context);
        try
        {
            var source = contextWrapper.SqlDatabaseFactory.GetDatabase(contextWrapper.SourceDatabaseName);

            var prevMode = context.SqlDatabaseFactory.Server.ConnectionContext.SqlExecutionModes;

            context.SqlDatabaseFactory.Server.ConnectionContext.SqlExecutionModes =
                    SqlExecutionModes.ExecuteAndCaptureSql;

            var target = contextWrapper.SqlDatabaseFactory.GetOrCreateDatabase(contextWrapper.DatabaseName);

            context.SqlDatabaseFactory.Server.ConnectionContext.SqlExecutionModes = prevMode;

            var tryCreateScript = source != null
                                          ? []
                                          : context.SqlDatabaseFactory.Server.ConnectionContext.CapturedSql.GetScriptsForBatchExecuting();

            this.TransferSchema(source, target);

            var tryCreateDictionary = new Dictionary<ApplyMigrationDbScriptMode, Lazy<IEnumerable<string>>>();

            if (tryCreateScript.Any())
            {
                tryCreateDictionary.Add(ApplyMigrationDbScriptMode.PreAddOrUpdate, new Lazy<IEnumerable<string>>(() => tryCreateScript));
            }

            var tryCreateResult = DatabaseScriptResultFactory.Create(tryCreateDictionary);

            var result = new[] { tryCreateResult, this.source.GenerateScript(contextWrapper) }.Combine();

            var evaluated = result.Evaluate();

            var wrapperResult = new DatabaseScriptResultDecorator(
                                                                  evaluated,
                                                                  z =>
                                                                  {
                                                                      var row = z.Replace(target.Name, contextWrapper.SourceDatabaseName.Name).Replace("$Database", contextWrapper.SourceDatabaseName.Name);
                                                                      var createViewValue = "createview";
                                                                      if (row.ToLower().Trim().Where(q => q != ' ').Take(createViewValue.Length).SequenceEqual(createViewValue))
                                                                      {
                                                                          row = new[] { ScriptsHelper.KeywordGo, row, ScriptsHelper.KeywordGo }.Join(Environment.NewLine);
                                                                      }

                                                                      return row;
                                                                  });

            if (this.removeSchemaDatabase)
            {
                context.SqlDatabaseFactory.Server.KillDatabase(contextWrapper.SqlDatabaseFactory.GetOrCreateDatabase(contextWrapper.DatabaseName).Name);
            }

            return wrapperResult;
        }
        finally
        {
            context.SqlDatabaseFactory.Server.ConnectionContext.CapturedSql.Clear();
        }
    }

    private IEnumerable<string> UnsafeEvaluate(IDatabaseScriptGeneratorContext context, Func<IEnumerable<string>> func, bool isFinally = false)
    {
        try
        {
            return func();
        }
        catch (Exception)
        {
            if (!isFinally)
            {
                context.SqlDatabaseFactory.Server.KillDatabase(context.SqlDatabaseFactory.GetOrCreateDatabase(context.DatabaseName).Name);
                context.SqlDatabaseFactory.Server.ConnectionContext.CapturedSql.Clear();
            }

            throw;
        }
        finally
        {
            if (isFinally)
            {
                context.SqlDatabaseFactory.Server.KillDatabase(context.SqlDatabaseFactory.GetOrCreateDatabase(context.DatabaseName).Name);
                context.SqlDatabaseFactory.Server.ConnectionContext.CapturedSql.Clear();
            }
        }
    }

    private void TransferSchema(Microsoft.SqlServer.Management.Smo.Database? database, Microsoft.SqlServer.Management.Smo.Database target)
    {
        if (null == database)
        {
            return;
        }

        /*
         * Трансфер уже выполнен.
         * Это происходит в том случае, когда генерация всех систем
         * выполняется в одну базу.
         */
        if (target.Tables.Count > 0)
        {
            return;
        }

        var copySchemaNames = database.Schemas
                                      .OfType<Schema>()
                                      .Select(s => s.Name)
                                      .Except(target.Schemas.OfType<Schema>()
                                                    .Select(s => s.Name))
                                      .ToArray();

        foreach (var sourceSchema in database.Schemas.OfType<Schema>().Where(s => copySchemaNames.Contains(s.Name)))
        {
            var schema = new Schema(target, sourceSchema.Name);
            schema.Create();
        }

        var transfer = new Transfer(database)
                       {
                               CopyAllObjects = false,
                               Options =
                               {
                                       WithDependencies = true,
                                       Default = true,
                                       Indexes = true,
                                       DriDefaults = true,
                                       DriAllKeys = true,
                                       DriForeignKeys = true,
                                       DriIndexes = true,
                                       DriPrimaryKey = true,
                                       DriUniqueKeys = true,
                                       ContinueScriptingOnError = true
                               },
                               DestinationDatabase = target.Name,
                               DestinationServer = target.Parent.Name,
                               DestinationLoginSecure = database.Parent.ConnectionContext.LoginSecure,
                               CreateTargetDatabase = false,
                               CopySchema = true,
                               CopyAllTables = true,
                               CopyAllPartitionFunctions = true,
                               CopyAllStoredProcedures = true,
                               CopyAllUserDefinedDataTypes = true,
                               CopyAllUserDefinedTableTypes = true,
                               CopyAllUserDefinedFunctions = true,
                               CopyAllViews = true,
                               CopyData = this.copyData,
                               CopyAllLogins = false,
                               CopyAllUsers = false,
                               CopyAllRules = false,
                               CopyAllRoles = false,
                       };

        transfer.CreateTargetDatabase = false;

        transfer.TransferData();

        // Copy data from [ExecutedScripts] tables
        if (null != this.copyDataForTables)
        {
            var transfer2 = new Transfer(database)
                            {
                                    CopyAllObjects = false,
                                    Options =
                                    {
                                            WithDependencies = true,
                                            Default = true,
                                            Indexes = true,
                                            DriDefaults = true,
                                            DriAllKeys = true,
                                            DriForeignKeys = true,
                                            DriIndexes = true,
                                            DriPrimaryKey = true,
                                            DriUniqueKeys = true,
                                    },
                                    DestinationDatabase = target.Name,
                                    DestinationServer = target.Parent.Name,
                                    DestinationLoginSecure = database.Parent.ConnectionContext.LoginSecure,
                                    CreateTargetDatabase = false,
                                    CopySchema = false,
                                    CopyAllTables = false,
                                    CopyData = true,
                                    CopyAllLogins = false,
                                    CopyAllUsers = false,
                                    CopyAllRules = false,
                                    CopyAllRoles = false,
                            };

            var copyTables = (from schema in database.Schemas.OfType<Schema>()
                              from tableName in this.copyDataForTables
                              select database.Tables[tableName, schema.Name]).Where(t => t != null).ToArray();

            foreach (var copyTable in copyTables)
            {
                transfer2.ObjectList.Add(copyTable);
            }

            if (copyTables.Any())
            {
                transfer2.TransferData();
            }
        }
    }

    internal class DatabaseScriptGeneratorContextWrapper : IDatabaseScriptGeneratorContext
    {
        private static readonly string PostFix = DateTime.Now.ToString();

        private readonly IDatabaseScriptGeneratorContext source;

        public DatabaseScriptGeneratorContextWrapper(IDatabaseScriptGeneratorContext source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            this.source = source;
        }

        public DatabaseName DatabaseName =>

            this.source.DatabaseName with { Name = this.source.DatabaseName.Name + "_" + PostFix + "_GEN" };

        public DatabaseName SourceDatabaseName => this.source.DatabaseName;

        public ISqlDatabaseFactory SqlDatabaseFactory => this.source.SqlDatabaseFactory;

        public AssemblyMetadata AssemblyMetadata => this.source.AssemblyMetadata;
    }
}
