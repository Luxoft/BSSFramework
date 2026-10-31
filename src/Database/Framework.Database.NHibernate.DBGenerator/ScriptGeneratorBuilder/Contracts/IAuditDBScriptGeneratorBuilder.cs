using Framework.Database.NHibernate.Mapping;

namespace Framework.Database.NHibernate.DBGenerator.ScriptGeneratorBuilder.Contracts;

public interface IAuditDBScriptGeneratorBuilder : IConfigurable
{
    IAuditDBScriptGeneratorBuilder WithAuditPostfix(string auditTablePostfix = "Audit");

    IAuditDBScriptGeneratorBuilder WithMappingSettings(MappingSettings mappingSettings);

    IAuditDBScriptGeneratorBuilder WithMappingSettings(List<MappingSettings> mappingSettings);

    /// <summary>
    /// Setting that prevents deletion of the temporary DB intended for applying the schema.
    /// </summary>
    /// <returns><see cref="IAuditDBScriptGeneratorBuilder"/>.</returns>
    IAuditDBScriptGeneratorBuilder WithPreserveSchemaDatabase();

    IMigrationScriptGeneratorBuilder MigrationBuilder { get; }
}
