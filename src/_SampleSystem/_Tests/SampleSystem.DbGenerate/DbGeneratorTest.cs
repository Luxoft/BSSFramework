using Framework.DomainDriven;
using Framework.DomainDriven.DBGenerator;
using Framework.DomainDriven.NHibernate;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.CodeGenerate;
using SampleSystem.Generated.DAL.NHibernate;

namespace SampleSystem.DbGenerate;

[TestClass]
public class DbGeneratorTest
{
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
        UserCredential credential = null,
        params string[] migrationScriptFolderPaths)
    {
        if (!skipFrameworkDatabases)
        {
            this.GenerateAuthorizationDatabase(
                serverName,
                new DatabaseName(mainDatabaseName, "auth"),
                new DatabaseName(mainDatabaseName, "auth").ToDefaultAudit(),
                mode,
                true,
                credential);

            this.GenerateConfigurationDatabase(
                serverName,
                new DatabaseName(mainDatabaseName, "configuration"),
                new DatabaseName(mainDatabaseName, "configuration").ToDefaultAudit(),
                mode,
                true,
                credential);
        }

        var result = this.GenerateSampleSystemDB(
            serverName,
            new DatabaseName(mainDatabaseName, "app"),
            new DatabaseName(mainDatabaseName, "app").ToDefaultAudit(),
            mode: mode,
            generatorMode: generatorMode,
            migrationScriptFolderPaths: migrationScriptFolderPaths,
            preserveSchemaDatabase: true,
            ignoredIndexes: ignoredIndexes,
            credential: credential);

        return result;
    }

    private string GenerateSampleSystemDB(
        string serverName,
        DatabaseName databaseName,
        AuditDatabaseName auditDatabaseName,
        DatabaseScriptGeneratorMode generatorMode = DatabaseScriptGeneratorMode.AutoGenerateUpdateChangeTypeScript,
        DBGenerateScriptMode mode = DBGenerateScriptMode.AppliedOnTargetDatabase,
        IEnumerable<string> migrationScriptFolderPaths = null,
        IEnumerable<string> auditMigrationScriptFolderPaths = null,
        bool preserveSchemaDatabase = false,
        ICollection<string> ignoredIndexes = null,
        UserCredential credential = null)
    {
        var generator = new SampleSystemDBGenerator(this.GetMappingSettings(serverName, databaseName, auditDatabaseName));

        var result = generator.Generate(
            serverName,
            mode: mode,
            generatorMode: generatorMode,
            migrationScriptFolderPaths: migrationScriptFolderPaths,
            auditMigrationScriptFolderPaths: auditMigrationScriptFolderPaths,
            preserveSchemaDatabase: preserveSchemaDatabase,
            ignoredIndexes: ignoredIndexes,
            credentials: credential);

        var lines = result.ToNewLinesCombined();
        return lines;
    }

    private void GenerateConfigurationDatabase(
        string serverName,
        DatabaseName mainDatabaseName,
        AuditDatabaseName auditDatabaseName,
        DBGenerateScriptMode mode = DBGenerateScriptMode.AppliedOnCopySchemeDatabase,
        bool preserveSchemaDatabase = false,
        UserCredential credential = null)
    {
        string[] migrationScriptFolderPaths = null;

        Console.WriteLine("------ start Utilities");
        var resultScript = new Framework.Configuration.TestGenerate.ServerGenerators(
                new Framework.Configuration.TestGenerate.ServerGenerationEnvironment(new DatabaseName("Configuration")))
            .GenerateDB(
                serverName,
                mainDatabaseName,
                auditDatabaseName,
                migrationScriptFolderPaths: migrationScriptFolderPaths,
                mode: mode,
                preserveSchemaDatabase: preserveSchemaDatabase,
                credentials: credential);

        var lines = resultScript;
        Console.WriteLine("------ end Utilities");
        Console.WriteLine(lines);
    }

    private void GenerateAuthorizationDatabase(
        string serverName,
        DatabaseName mainDatabaseName,
        AuditDatabaseName auditDatabaseName,
        DBGenerateScriptMode mode = DBGenerateScriptMode.AppliedOnCopySchemeDatabase,
        bool preserveSchemaDatabase = false,
        UserCredential credential = null)
    {
        string[] migrationScriptFolderPaths = null;
        var result = new Framework.Authorization.TestGenerate.ServerGenerators().GenerateDB(
            serverName,
            mainDatabaseName,
            auditDatabaseName,
            migrationScriptFolderPaths: migrationScriptFolderPaths,
            mode: mode,
            preserveSchemaDatabase: preserveSchemaDatabase,
            credentials: credential);

        Console.WriteLine(result);
    }

    private MappingSettings GetMappingSettings(string serverName, DatabaseName dbName, AuditDatabaseName dbAuditName)
    {
        var mappingXmls = this.environment.DAL.GetMappingGenerators().Select(mg => mg.Generate());

        var connectionString = $"Data Source={serverName};Initial Catalog={dbName.Name};Application Name=SampleSystem";

        return new SampleSystemMappingSettings(mappingXmls, dbName, dbAuditName)
            .AddInitializer(new SampleSystemConfigurationInitializer(connectionString));
    }
}
