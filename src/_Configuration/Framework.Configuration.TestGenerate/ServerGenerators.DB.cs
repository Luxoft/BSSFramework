using Framework.DomainDriven;
using Framework.DomainDriven.DBGenerator;

namespace Framework.Configuration.TestGenerate;

public partial class ServerGenerators
{
    public string GenerateDB(
            string serverName,
            DatabaseScriptGeneratorMode generatorMode = DatabaseScriptGeneratorMode.AutoGenerateUpdateChangeTypeScript,
            DBGenerateScriptMode mode = DBGenerateScriptMode.AppliedOnTargetDatabase,
            IEnumerable<string>? migrationScriptFolderPaths = null,
            IEnumerable<string>? auditMigrationScriptFolderPaths = null,
            DbUserCredential? credentials = null)
    {
        var generator = new DBGenerator(this.Environment.MappingSettings);
        var result = generator.Generate(
                                        serverName,
                                        mode: mode,
                                        generatorMode: generatorMode,
                                        migrationScriptFolderPaths: migrationScriptFolderPaths,
                                        auditMigrationScriptFolderPaths: auditMigrationScriptFolderPaths,
                                        credentials: credentials);

        var lines = result.ToNewLinesCombined();
        return lines;
    }

    public string GenerateDB(
            string serverName,
            DatabaseName databaseName,
            DatabaseScriptGeneratorMode generatorMode = DatabaseScriptGeneratorMode.AutoGenerateUpdateChangeTypeScript,
            DBGenerateScriptMode mode = DBGenerateScriptMode.AppliedOnTargetDatabase,
            IEnumerable<string>? migrationScriptFolderPaths = null,
            IEnumerable<string>? auditMigrationScriptFolderPaths = null,
            bool preserveSchemaDatabase = false,
            DbUserCredential? credentials = null)
    {
        var generator = new DBGenerator(this.Environment.GetMappingSettings(databaseName));
        var result = generator.Generate(
                                        serverName,
                                        mode: mode,
                                        generatorMode: generatorMode,
                                        migrationScriptFolderPaths: migrationScriptFolderPaths,
                                        auditMigrationScriptFolderPaths: auditMigrationScriptFolderPaths,
                                        preserveSchemaDatabase: preserveSchemaDatabase,
                                        credentials: credentials);

        var lines = result.ToNewLinesCombined();
        return lines;
    }
}
