using Framework.Database.NHibernate._MappingSettings;
using Framework.Database.NHibernate.DBGenerator.AuditDBGenerator;
using Framework.Database.NHibernate.DBGenerator.Contracts;
using Framework.Database.NHibernate.DBGenerator.ScriptGeneratorBuilder.Contracts;
using Framework.Database.NHibernate.DBGenerator.Team;

namespace Framework.Database.NHibernate.DBGenerator.ScriptGeneratorBuilder.Impl;

class AuditDBScriptGeneratorBuilder : IAuditDBScriptGeneratorBuilder
{
    private string auditPostfix = "Audit";
    private List<MappingSettings> mappingSettings;
    private readonly MigrationDBScriptGeneratorBuilder migrationBuilder = new();

    private bool removeSchemaDatabase = true;

    public IAuditDBScriptGeneratorBuilder WithAudit(MappingSettings mappingSettings, string auditTablePostfix = "audit") => this.WithAudit([mappingSettings], auditTablePostfix);

    public IAuditDBScriptGeneratorBuilder WithAudit(IEnumerable<MappingSettings> mappingSettings, string auditPostfix = "audit")
    {
        this.ValidateConfigurate();

        if (this.auditPostfix != null && !string.Equals(this.auditPostfix, auditPostfix))
        {
            throw new ArgumentException("AuditBulder can work with one db only");
        }
        this.auditPostfix = auditPostfix;

        return this;
    }

    public IAuditDBScriptGeneratorBuilder WithAuditPostfix(string auditPostfix = "Audit")
    {
        this.ValidateConfigurate();

        this.auditPostfix = auditPostfix;

        return this;
    }

    public IAuditDBScriptGeneratorBuilder WithMappingSettings(MappingSettings mappingSettings) => this.WithMappingSettings([mappingSettings]);

    public IAuditDBScriptGeneratorBuilder WithMappingSettings(List<MappingSettings> mappingSettings)
    {
        this.ValidateConfigurate();

        this.mappingSettings.ValidateOneSet(mappingSettings, "mappingSettings");

        this.mappingSettings = mappingSettings;

        return this;
    }

    public IMigrationScriptGeneratorBuilder MigrationBuilder => this.migrationBuilder;

    public IAuditDBScriptGeneratorBuilder WithPreserveSchemaDatabase()
    {
        this.removeSchemaDatabase = false;
        return this;
    }

    public IDatabaseScriptGenerator Build(DBGenerateScriptMode mode)
    {
        if (this.mappingSettings == null)
        {
            return EmptyDatabaseScriptGenerator.Value;
        }

        IDatabaseScriptGenerator result;

        var nextMappingSettings = this.mappingSettings;

        var auditDatabaseScriptGenerator = new AuditDatabaseScriptGenerator(nextMappingSettings, this.auditPostfix);
        var migrationBuilder = this.migrationBuilder.Build(mode);
        var combined = new[] {auditDatabaseScriptGenerator, migrationBuilder}.Combine();

        switch (mode)
        {
            case DBGenerateScriptMode.AppliedOnCopySchemeDatabase:
            {
                result = combined.Unsafe(false, this.removeSchemaDatabase);
                break;
            }
            case DBGenerateScriptMode.AppliedOnCopySchemeAndDataDatabase:
            {
                result = combined.Unsafe(false, this.removeSchemaDatabase);
                break;
            }
            default:
            {
                result = new []{ auditDatabaseScriptGenerator.ToTryCreateDatabase(), migrationBuilder}.Combine();
                break;
            }
        }

        return new ReplaceDatabaseNameDecorator(context => nextMappingSettings.First().AuditDatabase, result);
    }

    public bool IsFreezed { get; set; }
}
