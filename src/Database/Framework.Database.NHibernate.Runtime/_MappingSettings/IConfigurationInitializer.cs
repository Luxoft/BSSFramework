

using NHibernate.Cfg;

namespace Framework.Database.NHibernate._MappingSettings;

public interface IConfigurationInitializer
{
    void Initialize(Configuration cfg);
}
