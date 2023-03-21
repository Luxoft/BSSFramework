using System;
using System.Collections.Generic;
using System.Linq;
using Framework.DomainDriven.DBGenerator.Contracts;
using Framework.DomainDriven.DBGenerator.Team;
using Framework.DomainDriven.NHibernate;

namespace Framework.DomainDriven.DBGenerator;

class AuditDBScriptGeneratorBuilder : IAuditDBScriptGeneratorBuilder
{
    private string _auditPostfix = "Audit";
    private IList<IMappingSettings> _mappingSettings;
    private readonly MigrationDBScriptGeneratorBuilder _migrationBuilder;

    private bool removeSchemaDatabase = true;

    public AuditDBScriptGeneratorBuilder()
    {
        this._migrationBuilder = new MigrationDBScriptGeneratorBuilder();
    }


    public IAuditDBScriptGeneratorBuilder WithAudit(IMappingSettings mappingSettings, string auditTablePostfix = "audit")
    {
        return this.WithAudit(new[] { mappingSettings }, auditTablePostfix);
    }

    public IAuditDBScriptGeneratorBuilder WithAudit(IEnumerable<IMappingSettings> mappingSettings, string auditPostfix = "audit")
    {
        this.ValidateConfigurate();

        if (this._auditPostfix != null && !string.Equals(this._auditPostfix, auditPostfix))
        {
            throw new ArgumentException("AuditBulder can work with one db only");
        }
        this._auditPostfix = auditPostfix;

        return this;
    }

    public IAuditDBScriptGeneratorBuilder WithAuditPostfix(string auditPostfix = "Audit")
    {
        this.ValidateConfigurate();

        this._auditPostfix = auditPostfix;

        return this;
    }

    public IAuditDBScriptGeneratorBuilder WithMappingSettings(IMappingSettings mappingSettings)
    {
        return this.WithMappingSettings(new[] {mappingSettings});
    }

    public IAuditDBScriptGeneratorBuilder WithMappingSettings(IList<IMappingSettings> mappingSettings)
    {
        this.ValidateConfigurate();

        this._mappingSettings.ValidateOneSet(mappingSettings, "mappingSettings");

        this._mappingSettings = mappingSettings;

        return this;
    }

    public IMigrationScriptGeneratorBuilder MigrationBuilder => this._migrationBuilder;

    public IAuditDBScriptGeneratorBuilder WithPreserveSchemaDatabase()
    {
        this.removeSchemaDatabase = false;
        return this;
    }

    public IDatabaseScriptGenerator Build(DBGenerateScriptMode mode)
    {
        if (this._mappingSettings == null)
        {
            return EmptyDatabaseScriptGenerator.Value;
        }

        IDatabaseScriptGenerator result;

        var nextMappingSettings = this._mappingSettings;

        var auditDatabaseScriptGenerator = new AuditDatabaseScriptGenerator(nextMappingSettings, this._auditPostfix);
        var migrationBuilder = this._migrationBuilder.Build(mode);
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
