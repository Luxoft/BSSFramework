using System.Collections.Generic;

using Framework.DomainDriven.NHibernate;

namespace Framework.DomainDriven.DBGenerator;

public interface IAuditDBScriptGeneratorBuilder : IConfigurable
{
    IAuditDBScriptGeneratorBuilder WithAuditPostfix(string auditTablePostfix = "Audit");

    IAuditDBScriptGeneratorBuilder WithMappingSettings(IMappingSettings mappingSettingss);

    IAuditDBScriptGeneratorBuilder WithMappingSettings(IList<IMappingSettings> mappingSettings);

    /// <summary>
    /// Настройка, предотвращающая удаление временной БД, предназначенной для накатывания схемы.
    /// </summary>
    /// <returns><see cref="IAuditDBScriptGeneratorBuilder"/>.</returns>
    IAuditDBScriptGeneratorBuilder WithPreserveSchemaDatabase();

    IMigrationScriptGeneratorBuilder MigrationBuilder { get; }
}
