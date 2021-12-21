using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.Core.Services;
using Framework.DomainDriven.DBGenerator.Contracts;
using Framework.DomainDriven.Metadata;
using Framework.DomainDriven.NHibernate;

using JetBrains.Annotations;

namespace Framework.DomainDriven.DBGenerator
{
    public class DBGenerator
    {
        private readonly IMappingSettings _settings;

        public DBGenerator([NotNull] IMappingSettings settings) =>
            this._settings = settings ?? throw new ArgumentNullException(nameof(settings));

        protected virtual void Init(
            DatascriptGeneratorBuilder builder,
            DatabaseScriptGeneratorMode mode = DatabaseScriptGeneratorMode.AutoGenerateUpdateChangeTypeScript,
            ICollection<string> ignoredIndexes = null)
        {
            builder.MainBuilder.WithMain(mode, ignoredIndexes: ignoredIndexes);
            builder.MainBuilder.WithRequireRef(this.GetIgnoreLinks().ToArray());
            builder.MainBuilder.WithUniqueGroup();
        }

        protected virtual IEnumerable<IgnoreLink> GetIgnoreLinks()
        {
            yield break;
        }

        /// <summary>
        /// Generates the specified server name.
        /// </summary>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="userAuthenticationService">The user authentication service.</param>
        /// <param name="generatorMode">The generator mode.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="updateScriptsDB">The update scripts database.</param>
        /// <param name="executedScriptsTable">The executed scripts table.</param>
        /// <param name="migrationScriptFolderPaths">The migration script folder paths.</param>
        /// <param name="auditMigrationScriptFolderPaths">The audit migration script folder paths.</param>
        /// <param name="preserveSchemaDatabase">if set to <c>true</c> [preserve schema database].</param>
        /// <param name="customMigrationScriptReader">Custom migration scripts reader</param>
        /// <param name="ignoredIndexes">The ignored indexes.</param>
        /// <param name="credentials">it will be used for connection</param>
        /// <returns>IDatabaseScriptResult.</returns>
        public IDatabaseScriptResult Generate(
            string serverName,
            IUserAuthenticationService userAuthenticationService,
            DatabaseScriptGeneratorMode generatorMode = DatabaseScriptGeneratorMode.AutoGenerateUpdateChangeTypeScript,
            DBGenerateScriptMode mode = DBGenerateScriptMode.AppliedOnTargetDatabase,
            string databaseName = null,
            string updateScriptsDB = null,
            string executedScriptsTable = "ExecutedScripts",
            IEnumerable<string> migrationScriptFolderPaths = null,
            IEnumerable<string> auditMigrationScriptFolderPaths = null,
            bool preserveSchemaDatabase = false,
            IMigrationScriptReader customMigrationScriptReader = null,
            ICollection<string> ignoredIndexes = null,
            UserCredential credentials = null)
        {
            migrationScriptFolderPaths ??= new string[0];
            auditMigrationScriptFolderPaths ??= new string[0];

            var dbName = this._settings.Database;

            if (null != databaseName)
            {
                dbName = new DatabaseName(databaseName);
            }

            var scriptDBName = updateScriptsDB ?? dbName.Name;

            var metadata = MetadataReader.GetAssemblyMetadata(this._settings.PersistentDomainObjectBaseType, this._settings.GetDomainTypeAssemblies().ToArray(a => AssemblyInfo.Create(a)));

            this.FilterMetadata(metadata);

            var builder = new DatascriptGeneratorBuilder(mode, userAuthenticationService);

            this.Init(builder, generatorMode, ignoredIndexes);

            if (this._settings.IsAudited())
            {
                builder.AuditBuilder.WithAuditPostfix();
                builder.AuditBuilder.WithMappingSettings(this._settings);
                auditMigrationScriptFolderPaths.Foreach(z => builder.AuditBuilder.MigrationBuilder.WithFolder(z));
            }

            migrationScriptFolderPaths.Foreach(z => builder.MainBuilder.MigrationBuilder.WithFolder(z));

            if (customMigrationScriptReader != null)
            {
                builder.MainBuilder.MigrationBuilder.WithCustom(customMigrationScriptReader);
            }

            builder.MainBuilder.MigrationBuilder.WithDatabase(updateScriptsDB).WithTable(executedScriptsTable);
            builder.MainBuilder.MigrationBuilder.WithDatabase(scriptDBName);
            builder.MainBuilder.MigrationBuilder.WithTable(executedScriptsTable);

            builder.WithMainDatabase(dbName);
            if (credentials != null)
            {
                builder.WithServerName(serverName, credentials);
            }
            else
            {
                builder.WithServerName(serverName);
            }

            builder.WithAssemblyMetadata(metadata);

            if (this._settings.IsAudited() && this._settings.IsAuditInMainDatabase())
            {
                builder.MainBuilder.WithPreserveSchemaDatabase();
            }

            if (this._settings.IsAudited() && preserveSchemaDatabase)
            {
                builder.AuditBuilder.WithPreserveSchemaDatabase();
            }

            var resultScript = builder.GenerateScript();

            return resultScript;
        }

        /// <summary>
        /// Metadata filter. Do not filter anything by default
        /// </summary>
        /// <remarks>
        /// Filter metadata if necessary in overriden method. Something like this:
        /// <code><![CDATA[
        ///      metadata.DomainTypes = metadata
        ///                         .DomainTypes
        ///                         .Where(z => !z.DomainType.GetCustomAttributes<TableAttribute>().Any())
        ///                         .ToList();
        ///  ]]></code>
        /// </remarks>
        /// <param name="metadata">Metadata instance to filter DomainTypes</param>
        protected virtual void FilterMetadata(AssemblyMetadata metadata)
        {
        }
    }
}
