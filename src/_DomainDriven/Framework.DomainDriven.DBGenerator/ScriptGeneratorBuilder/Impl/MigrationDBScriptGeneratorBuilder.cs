using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.DBGenerator.Contracts;
using Framework.DomainDriven.DBGenerator.Team;

using JetBrains.Annotations;

using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Framework.DomainDriven.DBGenerator;

internal class MigrationDBScriptGeneratorBuilder : IMigrationScriptGeneratorBuilder
{
    private IList<IMigrationScriptReader> _readers = new List<IMigrationScriptReader>();
    private string _tableName = "ExecutedScripts";

    public bool IsFreezed { get; internal set; }

    public string TableName
    {
        get { return this._tableName; }
        set { this._tableName = value; }
    }

    public IDatabaseScriptGenerator Build(DBGenerateScriptMode scriptMode)
    {
        var result = this._readers.Combine();

        IDatabaseScriptGenerator scriptAdapter = new DatabaseScriptGeneratorAdapter(result, this._tableName);

        return scriptAdapter;
    }

    public IMigrationScriptGeneratorBuilder WithCustom(IMigrationScriptReader source)
    {
        this.ValidateConfigurate();

        this._readers.Add(source);

        return this;
    }

    public IMigrationScriptGeneratorBuilder WithDatabase(string databaseName)
    {
        return this;
    }

    public IMigrationScriptGeneratorBuilder WithFolder(string directoryPath)
    {
        this.ValidateConfigurate();

        this._readers.Add(new FileScriptReader(directoryPath));

        return this;
    }

    public IMigrationScriptGeneratorBuilder WithTable(string tableName)
    {
        this.ValidateConfigurate();

        this._tableName = tableName;

        return this;
    }

    protected class DatabaseScriptGeneratorAdapter : IDatabaseScriptGenerator
    {
        private readonly IMigrationScriptReader _migrationScriptReader;
        private readonly string _tableName;

        public DatabaseScriptGeneratorAdapter(
                [NotNull] IMigrationScriptReader migrationScriptReader,
                string tableName)
        {
            if (migrationScriptReader == null)
            {
                throw new ArgumentNullException(nameof(migrationScriptReader));
            }

            this._migrationScriptReader = migrationScriptReader;
            this._tableName = tableName;
        }

        public IDatabaseScriptResult GenerateScript(IDatabaseScriptGeneratorContext context)
        {
            var createExecutingScriptsTable = this.TryCreateExecutionScriptTable(context);

            var executedMigrationScripts = new HashSet<MigrationDbScriptHeader>();

            if (createExecutingScriptsTable == null)
            {
                executedMigrationScripts = this.GetExecutedMigrationScripts(context).ToHashSet();
            }

            var allMigrationScripts = this._migrationScriptReader.Read();

            var migrationScripts = allMigrationScripts
                                   .Where(z => z.RunAlways || !executedMigrationScripts.Contains(z.Header))
                                   .ToList();

            var newScripts = allMigrationScripts.Where(z => !executedMigrationScripts.Contains(z.Header)).Select(z => z.Header).ToHashSet();

            var dictionary = new Dictionary<ApplyMigrationDbScriptMode, Lazy<IEnumerable<string>>>();

            var combinedScirpts = migrationScripts.Select(z => new { NeedExecute = true, Source = this.InjectDatabaseName(z, context) }).ToList();
            if (createExecutingScriptsTable.HasValue)
            {
                combinedScirpts.Add(new { NeedExecute = false, Source = this.InjectDatabaseName(createExecutingScriptsTable.Value, context) });
            }

            foreach (var grouped in combinedScirpts.GroupBy(z => z.Source.ApplyCustomScriptMode).OrderBy(z => z.Key))
            {
                var values = grouped.ToList();

                dictionary.Add(grouped.Key, new Lazy<IEnumerable<string>>(
                                                                          () =>
                                                                          {
                                                                              values.Where(z => z.NeedExecute).Foreach(q => this.Execute(q.Source.Script, context));

                                                                              var insertedScripts = values.Where(z => newScripts.Contains(z.Source.Header)).Select(
                                                                               z =>
                                                                               {
                                                                                   var result = this.GetInsertScript(z.Source, context);
                                                                                   this.Execute(result, context);
                                                                                   return result;
                                                                               }).ToList();

                                                                              return values.Select(z => z.Source.Script).Concat(insertedScripts).SelectMany(z => new[] { z, ScriptsHelper.KeywordGo }).ToList();
                                                                          }));
            }


            return DatabaseScriptResultFactory.Create(dictionary);
        }

        private MigrationDbScript InjectDatabaseName(MigrationDbScript source, IDatabaseScriptGeneratorContext context)
        {
            return new MigrationDbScript(source.Header.Name, source.RunAlways, source.ApplyCustomScriptMode, source.Header.Scheme, source.Header.Version,
                                         new Lazy<string>(() => source.Script.Replace("$Database", context.DatabaseName.Name)));
        }

        private void Execute(string commandText, IDatabaseScriptGeneratorContext context)
        {
            try
            {
                using (var connection = context.SqlDatabaseFactory.Server.ConnectionContext.SqlConnectionObject)
                {
                    connection.Open();
                    ////var nextCommandText = commandText.Replace("GO", string.Empty);
                    var replaceTargetDBText = commandText.Replace("$Database", context.DatabaseName.Name);

                    context.SqlDatabaseFactory.Server.ConnectionContext.ExecuteNonQuery(replaceTargetDBText);

                    ////var command = new SqlCommand(replaceTargetDBText, connection);
                    ////command.CommandType = CommandType.Text;
                    ////command.ExecuteNonQuery();
                }

            }
            catch (Exception e)
            {
                throw new System.ArgumentException($"Failed execute:{commandText}", e);
            }
        }

        private IList<MigrationDbScriptHeader> GetExecutedMigrationScripts(IDatabaseScriptGeneratorContext context)
        {
            var result = new List<MigrationDbScriptHeader>();
            using (var connection = context.SqlDatabaseFactory.Server.ConnectionContext.SqlConnectionObject)
            {
                connection.Open();
                var cmd = $"select name, scheme, version from {context.DatabaseName}.[{this._tableName}]";
                var command = new Microsoft.Data.SqlClient.SqlCommand(cmd, connection);
                var sqlResult = command.ExecuteReader();

                while (sqlResult.Read())
                {
                    result.Add(new MigrationDbScriptHeader(
                                                           sqlResult.GetString(0),
                                                           sqlResult.GetString(1),
                                                           sqlResult.GetString(2)));
                }
            }

            return result;
        }

        private string GetInsertScript(MigrationDbScript migrationDbScript, IDatabaseScriptGeneratorContext context)
        {
            return
                    $"INSERT INTO {context.DatabaseName.ToString()}.[{this._tableName}] ([name] ,[author] ,[date], [script], [version], [scheme]) VALUES ('{migrationDbScript.Header.Name}', system_user, GETDATE(), '{migrationDbScript.Script.Replace("'", "''")}','{migrationDbScript.Header.Version}','{migrationDbScript.Header.Scheme}')";
        }

        private MigrationDbScript? TryCreateExecutionScriptTable(IDatabaseScriptGeneratorContext context)
        {
            var server = context.SqlDatabaseFactory.Server;

            server.ConnectionContext.SqlExecutionModes = SqlExecutionModes.ExecuteAndCaptureSql;

            var db = context.SqlDatabaseFactory.GetOrCreateDatabase(context.DatabaseName);

            server.ConnectionContext.SqlExecutionModes = SqlExecutionModes.ExecuteAndCaptureSql;

            db.Tables.Refresh();

            if (db.Tables.Contains(this._tableName, context.DatabaseName.Schema))
            {
                return null;
            }

            var table = new Table(db, this._tableName, context.DatabaseName.Schema);

            var columnsInfo = new[]
                              {
                                      new { Name = "name", Type = DataType.NVarChar(255) },
                                      new { Name = "author", Type = DataType.NVarChar(255) },
                                      new { Name = "date", Type = DataType.DateTime },
                                      new { Name = "script", Type = DataType.NText },
                                      new { Name = "version", Type = DataType.NVarChar(255) },
                                      new { Name = "scheme", Type = DataType.NVarChar(255) }
                              };

            var columns = columnsInfo.Select(z => new Column(table, z.Name, z.Type).Self(q => q.Nullable = false)).ToList();

            columns.Foreach(z => table.Columns.Add(z));

            table.Create();

            ////var keyIndex = new Index(table, "PK_" + this._tableName);

            ////keyIndex.IndexedColumns.Add(new IndexedColumn(keyIndex, columns[0].Name));
            ////keyIndex.IsUnique = true;
            ////keyIndex.IndexKeyType = IndexKeyType.DriPrimaryKey;
            ////table.Indexes.Add(keyIndex);

            ////keyIndex.Create();

            var resultScript = server.ConnectionContext.CapturedSql.GetScriptForBatchExecuting(false);

            return new MigrationDbScript(
                                         "CreateMigrationTable",
                                         false,
                                         ApplyMigrationDbScriptMode.PreAddOrUpdate,
                                         "empty",
                                         "1.0",
                                         resultScript);
        }
    }
}
