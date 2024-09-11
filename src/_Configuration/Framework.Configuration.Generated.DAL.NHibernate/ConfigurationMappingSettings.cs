using Framework.Configuration.Domain;
using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate;

namespace Framework.Configuration.Generated.DAL.NHibernate;

public class ConfigurationMappingSettings(DatabaseName databaseName) :
    MappingSettings<PersistentDomainObjectBase>(typeof(ConfigurationMappingSettings).Assembly, databaseName)
{
    public ConfigurationMappingSettings()
        : this(new DatabaseName("", "configuration"))
    {
    }
}
