using Framework.Database.Metadata;
using Framework.Database.NHibernate.DBGenerator.Contracts;
using Framework.Database.NHibernate.DBGenerator.ScriptGeneratorBuilder.Contracts;
using Framework.Database.NHibernate.DBGenerator.ScriptGeneratorBuilder.Impl;
using Framework.Database.NHibernate.DBGenerator.ScriptGenerators.Support;
using Framework.Database.NHibernate.DBGenerator.Team;

namespace Framework.Database.NHibernate.DBGenerator.ScriptGeneratorBuilder;

public class DataScriptGeneratorBuilder(DBGenerateScriptMode scriptMode)
{
    private readonly MainDBScriptGeneratorBuilder mainDbScriptGeneratorBuilder = new();
    private readonly AuditDBScriptGeneratorBuilder auditDbScriptGeneratorBuilder = new();

    private DatabaseName databaseName;

    private SqlDatabaseFactory sqlDatabaseFactory;

    private AssemblyMetadata assemblyMetadata;

    public IMainDBScriptGeneratorBuilder MainBuilder => this.mainDbScriptGeneratorBuilder;

    public IAuditDBScriptGeneratorBuilder AuditBuilder => this.auditDbScriptGeneratorBuilder;

    [Obsolete("Use MigrationBuilder from MainBuilder or AuditBuilder")]
    public IMigrationScriptGeneratorBuilder MigrationBuilder => this.MainBuilder.MigrationBuilder;

    private IDatabaseScriptGenerator Build()
    {
        var main = this.mainDbScriptGeneratorBuilder.Build(scriptMode);
        var audit = this.auditDbScriptGeneratorBuilder.Build(scriptMode);

        this.mainDbScriptGeneratorBuilder.IsFrozen = true;
        this.auditDbScriptGeneratorBuilder.IsFrozen = true;

        return new[] { main, audit }.Combine();
    }

    public DataScriptGeneratorBuilder WithMainDatabase(string newDatabaseName) =>
            this.WithMainDatabase(new DatabaseName(newDatabaseName));

    public DataScriptGeneratorBuilder WithMainDatabase(DatabaseName newDatabaseName)
    {
        this.databaseName.ValidateOneSet(newDatabaseName, nameof(newDatabaseName));

        this.databaseName = newDatabaseName;
        return this;
    }

    public DataScriptGeneratorBuilder WithSqlDatabaseFactory(SqlDatabaseFactory newSqlDatabaseFactory)
    {
        this.sqlDatabaseFactory.ValidateOneSet(newSqlDatabaseFactory, nameof(newSqlDatabaseFactory));

        this.sqlDatabaseFactory = newSqlDatabaseFactory;
        return this;
    }

    public DataScriptGeneratorBuilder WithServerName(string serverName) =>
            this.WithSqlDatabaseFactory(SqlDatabaseFactory.CreateDefault(serverName));

    public DataScriptGeneratorBuilder WithServerName(string serverName, DbUserCredential credentials) =>
            this.WithSqlDatabaseFactory(SqlDatabaseFactory.Create(serverName, credentials));

    public DataScriptGeneratorBuilder WithAssemblyMetadata(AssemblyMetadata newAssemblyMetadata)
    {
        this.assemblyMetadata.ValidateOneSet(newAssemblyMetadata, nameof(newAssemblyMetadata));

        this.assemblyMetadata = newAssemblyMetadata;
        return this;
    }


    public IDatabaseScriptResult GenerateScript()
    {
        this.ValidateConfiguration();

        var context = new DatabaseScriptGeneratorContext(this.databaseName, this.sqlDatabaseFactory, this.assemblyMetadata);

        return this.Build().GenerateScript(context);
    }

    private void ValidateConfiguration()
    {
        if (this.sqlDatabaseFactory == null)
        {
            throw new ArgumentException("SqlFactory or serverName must be initialized");
        }

        if (this.assemblyMetadata == null)
        {
            throw new ArgumentException("AssemblyMetadata must be initialized");
        }

        if (string.IsNullOrWhiteSpace(this.databaseName.Name))
        {
            throw new ArgumentException("databaseName must be initialized");
        }
    }
}
