using Framework.Configuration.Domain;
using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate;

namespace Framework.Configuration.Generated.DAL.NHibernate
{
    public class ConfigurationMappingSettings : MappingSettings<PersistentDomainObjectBase>
    {
        public ConfigurationMappingSettings(DatabaseName databaseName)
            : base(typeof(ConfigurationMappingSettings).Assembly, databaseName)
        {
        }

        public ConfigurationMappingSettings(DatabaseName databaseName, AuditDatabaseName auditDatabaseName)
            : base(typeof(ConfigurationMappingSettings).Assembly, databaseName, auditDatabaseName)
        {
        }

        /// <summary>
        ///     Дефолтный настроки базы данных
        ///     Аудит имеет схему configuration, ревизии хранятся в схеме appAudit
        /// </summary>
        public static ConfigurationMappingSettings CreateDefaultAudit(string mainDatabaseName)
        {
            var databaseName = new DatabaseName(mainDatabaseName, "configuration");

            return new ConfigurationMappingSettings(databaseName, databaseName.ToDefaultAudit());
        }
    }
}
