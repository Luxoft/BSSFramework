using Framework.DomainDriven.NHibernate;

namespace Framework.DomainDriven.DBGenerator;

public interface IAuditDBScriptGeneratorBuilder : IConfigurable
{
    IAuditDBScriptGeneratorBuilder WithAuditPostfix(string auditTablePostfix = "Audit");

    IAuditDBScriptGeneratorBuilder WithMappingSettings(MappingSettings mappingSettings);

    IAuditDBScriptGeneratorBuilder WithMappingSettings(IList<MappingSettings> mappingSettings);

    /// <summary>
    /// Настройка, предотвращающая удаление временной БД, предназначенной для накатывания схемы.
    /// </summary>
    /// <returns><see cref="IAuditDBScriptGeneratorBuilder"/>.</returns>
    IAuditDBScriptGeneratorBuilder WithPreserveSchemaDatabase();

    IMigrationScriptGeneratorBuilder MigrationBuilder { get; }
}
