using Framework.Configuration.Domain;
using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate;

namespace Framework.Configuration.Generated.DAL.NHibernate;

public record ConfigurationMappingSettings(DatabaseName DatabaseName) :
    MappingSettings<PersistentDomainObjectBase>(typeof(ConfigurationMappingSettings).Assembly, DatabaseName)
{
    public ConfigurationMappingSettings()
        : this(new DatabaseName("", "configuration"))
    {
    }
}
