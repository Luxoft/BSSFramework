using NHibernate.Cfg;

namespace Framework.Database.NHibernate.Mapping;

public interface IConfigurationInitializer
{
    void Initialize(Configuration cfg);
}
