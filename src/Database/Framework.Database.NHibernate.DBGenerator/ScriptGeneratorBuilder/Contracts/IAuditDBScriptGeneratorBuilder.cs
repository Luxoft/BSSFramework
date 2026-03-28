using Framework.Database.NHibernate._MappingSettings;

namespace Framework.Database.NHibernate.DBGenerator.ScriptGeneratorBuilder.Contracts;

public interface IAuditDBScriptGeneratorBuilder : IConfigurable
{
    IAuditDBScriptGeneratorBuilder WithAuditPostfix(string auditTablePostfix = "Audit");

    IAuditDBScriptGeneratorBuilder WithMappingSettings(MappingSettings mappingSettings);

    IAuditDBScriptGeneratorBuilder WithMappingSettings(List<MappingSettings> mappingSettings);

    /// <summary>
    /// Настройка, предотвращающая удаление временной БД, предназначенной для накатывания схемы.
    /// </summary>
    /// <returns><see cref="IAuditDBScriptGeneratorBuilder"/>.</returns>
    IAuditDBScriptGeneratorBuilder WithPreserveSchemaDatabase();

    IMigrationScriptGeneratorBuilder MigrationBuilder { get; }
}
