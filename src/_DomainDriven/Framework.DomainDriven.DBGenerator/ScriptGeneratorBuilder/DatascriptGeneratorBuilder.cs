using System;

using Framework.DomainDriven.DBGenerator.Contracts;
using Framework.DomainDriven.DBGenerator.Team;
using Framework.DomainDriven.Metadata;

namespace Framework.DomainDriven.DBGenerator
{
    public class DatascriptGeneratorBuilder
    {
        private readonly DBGenerateScriptMode scriptMode;

        private readonly MainDBScriptGeneratorBuilder mainDbScriptGeneratorBuilder;
        private readonly AuditDBScriptGeneratorBuilder auditDbScriptGeneratorBuilder;

        private DatabaseName databaseName;
        private SqlDatabaseFactory sqlDatabaseFactory;
        private AssemblyMetadata assemblyMetadata;


        public DatascriptGeneratorBuilder(DBGenerateScriptMode scriptMode)
        {
            this.scriptMode = scriptMode;

            this.mainDbScriptGeneratorBuilder = new MainDBScriptGeneratorBuilder();
            this.auditDbScriptGeneratorBuilder = new AuditDBScriptGeneratorBuilder();
        }

        public IMainDBScriptGeneratorBuilder MainBuilder => this.mainDbScriptGeneratorBuilder;

        public IAuditDBScriptGeneratorBuilder AuditBuilder => this.auditDbScriptGeneratorBuilder;

        [Obsolete("Use MigrationBuilder from MainBuilder or AuditBuilder")]
        public IMigrationScriptGeneratorBuilder MigrationBuilder => this.MainBuilder.MigrationBuilder;

        private IDatabaseScriptGenerator Build()
        {
            var main = this.mainDbScriptGeneratorBuilder.Build(this.scriptMode);
            var audit = this.auditDbScriptGeneratorBuilder.Build(this.scriptMode);

            this.mainDbScriptGeneratorBuilder.IsFreezed = true;
            this.auditDbScriptGeneratorBuilder.IsFreezed = true;

            return new[] { main, audit }.Combine();
        }

        public DatascriptGeneratorBuilder WithMainDatabase(string databaseName) =>
            this.WithMainDatabase(new DatabaseName(databaseName));

        public DatascriptGeneratorBuilder WithMainDatabase(DatabaseName databaseName)
        {
            this.databaseName.ValidateOneSet(databaseName, nameof(databaseName));

            this.databaseName = databaseName;

            return this;
        }

        public DatascriptGeneratorBuilder WithSqlDatabaseFactory(SqlDatabaseFactory sqlDatabaseFactory)
        {
            this.sqlDatabaseFactory.ValidateOneSet(sqlDatabaseFactory, nameof(sqlDatabaseFactory));

            this.sqlDatabaseFactory = sqlDatabaseFactory;

            return this;
        }

        public DatascriptGeneratorBuilder WithServerName(string serverName) =>
            this.WithSqlDatabaseFactory(SqlDatabaseFactory.CreateDefault(serverName));

        public DatascriptGeneratorBuilder WithServerName(string serverName, UserCredential credentials) =>
            this.WithSqlDatabaseFactory(SqlDatabaseFactory.Create(serverName, credentials));

        public DatascriptGeneratorBuilder WithAssemblyMetadata(AssemblyMetadata assemblyMetadata)
        {
            this.assemblyMetadata.ValidateOneSet(assemblyMetadata, nameof(assemblyMetadata));

            this.assemblyMetadata = assemblyMetadata;

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
}
