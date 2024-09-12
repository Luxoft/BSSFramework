using Framework.Configuration.Domain;
using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate;

namespace Framework.Configuration.Generated.DAL.NHibernate;

public class ConfigurationMappingSettings(DatabaseName databaseName)
    : MappingSettings<PersistentDomainObjectBase>(typeof(ConfigurationMappingSettings).Assembly, databaseName)
{
    /// <summary>
    ///     Дефолтный настроки базы данных
    ///     Аудит имеет схему configuration, ревизии хранятся в схеме appAudit
    /// </summary>
    public static ConfigurationMappingSettings CreateDefault(string mainDatabaseName)
    {
        var databaseName = new DatabaseName(mainDatabaseName, "configuration");

        return new ConfigurationMappingSettings(databaseName);
    }
}
