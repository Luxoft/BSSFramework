using Framework.Configuration.Domain;
using Framework.Database;
using Framework.Database.NHibernate.Mapping;

namespace Framework.Configuration.Generated.DAL.NHibernate;

public record ConfigurationMappingSettings(DatabaseName DatabaseName) :
    MappingSettings<PersistentDomainObjectBase>(typeof(ConfigurationMappingSettings).Assembly, DatabaseName)
{
    public ConfigurationMappingSettings()
        : this(new DatabaseName("", "configuration"))
    {
    }
}
