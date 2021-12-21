using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core.Services;
using Framework.DomainDriven;
using Framework.DomainDriven.DBGenerator;
using Framework.DomainDriven.NHibernate;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.CodeGenerate;
using SampleSystem.Generated.DAL.NHibernate;

namespace SampleSystem.DbGenerate
{
    [TestClass]
    public class DbGeneratorTest
    {
        private readonly IUserAuthenticationService userAuthenticationService = UserAuthenticationService.CreateFor("DbGenerator");

        private readonly ServerGenerationEnvironment environment = new();

        [TestMethod]
        public void GenerateLocal() => this.GenerateAllDB(@".");

        public void GenerateDatabase(DbGenerationOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.Server))
            {
                throw new ArgumentException("Server name is empty");
            }

            if (string.IsNullOrWhiteSpace(options.DataBase))
            {
                throw new ArgumentException("DataBase name is empty");
            }

            Console.WriteLine($"Generate database:'{options.DataBase}' on {options.Server}");

            this.GenerateAllDB(options.Server, options.DataBase);
        }

        public string GenerateAllDB(
                string serverName,
                string mainDatabaseName = nameof(SampleSystem),
                DatabaseScriptGeneratorMode generatorMode = DatabaseScriptGeneratorMode.AutoGenerateUpdateChangeTypeScript,
                DBGenerateScriptMode mode = DBGenerateScriptMode.AppliedOnTargetDatabase,
                ICollection<string> ignoredIndexes = null,
                bool skipFrameworkDatabases = false,
                params string[] migrationScriptFolderPaths)
        {
            if (!skipFrameworkDatabases)
            {
                this.GenerateAuthorizationDatabase(
                                                   serverName,
                                                   new DatabaseName(mainDatabaseName, "auth"),
                                                   new DatabaseName(mainDatabaseName, "auth").ToDefaultAudit(),
                                                   this.userAuthenticationService,
                                                   mode,
                                                   true);

                this.GenerateConfigurationDatabase(
                                                   serverName,
                                                   new DatabaseName(mainDatabaseName, "configuration"),
                                                   new DatabaseName(mainDatabaseName, "configuration").ToDefaultAudit(),
                                                   this.userAuthenticationService,
                                                   mode,
                                                   true);

                this.GenerateWorkflowDatabase(
                                              serverName,
                                              new DatabaseName(mainDatabaseName, "workflow"),
                                              new DatabaseName(mainDatabaseName, "workflow").ToDefaultAudit(),
                                              this.userAuthenticationService,
                                              mode,
                                              false);
            }

            var result = this.GenerateSampleSystemDB(
                                                     serverName,
                                                     new DatabaseName(mainDatabaseName, "app"),
                                                     new DatabaseName(mainDatabaseName, "app").ToDefaultAudit(),
                                                     this.userAuthenticationService,
                                                     mode: mode,
                                                     generatorMode: generatorMode,
                                                     migrationScriptFolderPaths: migrationScriptFolderPaths,
                                                     preserveSchemaDatabase: true,
                                                     ignoredIndexes: ignoredIndexes);

            return result;
        }

        private string GenerateSampleSystemDB(
                string serverName,
                DatabaseName databaseName,
                AuditDatabaseName auditDatabaseName,
                IUserAuthenticationService userAuthenticationService,
                DatabaseScriptGeneratorMode generatorMode = DatabaseScriptGeneratorMode.AutoGenerateUpdateChangeTypeScript,
                DBGenerateScriptMode mode = DBGenerateScriptMode.AppliedOnTargetDatabase,
                IEnumerable<string> migrationScriptFolderPaths = null,
                IEnumerable<string> auditMigrationScriptFolderPaths = null,
                bool preserveSchemaDatabase = false,
                ICollection<string> ignoredIndexes = null)
        {
            var generator = new SampleSystemDBGenerator(this.GetMappingSettings(serverName, databaseName, auditDatabaseName));

            var result = generator.Generate(
                                            serverName,
                                            userAuthenticationService,
                                            mode: mode,
                                            generatorMode: generatorMode,
                                            migrationScriptFolderPaths: migrationScriptFolderPaths,
                                            auditMigrationScriptFolderPaths: auditMigrationScriptFolderPaths,
                                            preserveSchemaDatabase: preserveSchemaDatabase,
                                            ignoredIndexes: ignoredIndexes);

            var lines = result.ToNewLinesCombined();
            return lines;
        }

        private void GenerateConfigurationDatabase(
                string serverName,
                DatabaseName mainDatabaseName,
                AuditDatabaseName auditDatabaseName,
                IUserAuthenticationService userAuthenticationService,
                DBGenerateScriptMode mode = DBGenerateScriptMode.AppliedOnCopySchemeDatabase,
                bool preserveSchemaDatabase = false)
        {
            string[] migrationScriptFolderPaths = null;

            Console.WriteLine("------ start Utilities");
            var resultScript = new Framework.Configuration.TestGenerate.ServerGenerators(
                     new Framework.Configuration.TestGenerate.ServerGenerationEnvironment(new DatabaseName("Configuration")))
                    .GenerateDB(
                                serverName,
                                mainDatabaseName,
                                auditDatabaseName,
                                userAuthenticationService,
                                migrationScriptFolderPaths: migrationScriptFolderPaths,
                                mode: mode,
                                preserveSchemaDatabase: preserveSchemaDatabase);

            var lines = resultScript;
            Console.WriteLine("------ end Utilities");
            Console.WriteLine(lines);
        }

        private void GenerateWorkflowDatabase(
                string serverName,
                DatabaseName mainDatabaseName,
                AuditDatabaseName auditDatabaseName,
                IUserAuthenticationService userAuthenticationService,
                DBGenerateScriptMode mode = DBGenerateScriptMode.AppliedOnCopySchemeDatabase,
                bool preserveSchemaDatabase = false)
        {
            string[] migrationScriptFolderPaths = null;

            var result = new Framework.Workflow.TestGenerate.ServerGenerators().GenerateDB(
             serverName,
             mainDatabaseName,
             auditDatabaseName,
             userAuthenticationService,
             migrationScriptFolderPaths: migrationScriptFolderPaths,
             mode: mode,
             generatorMode: DatabaseScriptGeneratorMode.AutoGenerateUpdateChangeTypeScript
                            | DatabaseScriptGeneratorMode.RemoveObsoleteColumns,
             preserveSchemaDatabase: preserveSchemaDatabase);

            Console.WriteLine(result);
        }

        private void GenerateAuthorizationDatabase(
                string serverName,
                DatabaseName mainDatabaseName,
                AuditDatabaseName auditDatabaseName,
                IUserAuthenticationService userAuthenticationService,
                DBGenerateScriptMode mode = DBGenerateScriptMode.AppliedOnCopySchemeDatabase,
                bool preserveSchemaDatabase = false)
        {
            string[] migrationScriptFolderPaths = null;
            var result = new Framework.Authorization.TestGenerate.ServerGenerators().GenerateDB(
             serverName,
             mainDatabaseName,
             auditDatabaseName,
             userAuthenticationService,
             migrationScriptFolderPaths: migrationScriptFolderPaths,
             mode: mode,
             preserveSchemaDatabase: preserveSchemaDatabase);

            Console.WriteLine(result);
        }

        private IMappingSettings GetMappingSettings(string serverName, DatabaseName dbName, AuditDatabaseName dbAuditName)
        {
            var initMappingAction = this.environment.DAL.GetMappingGenerators()
                                        .Select(mg => mg.Generate());
            return new SampleSystemMappingSettings(
                                                   initMappingAction,
                                                   dbName,
                                                   dbAuditName,
                                                   $"Data Source={serverName};Initial Catalog={dbName.Name};Integrated Security=True;Application Name=SampleSystem");
        }
    }
}
