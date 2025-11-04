

using NHibernate.Cfg;

namespace Framework.DomainDriven.NHibernate;

public class NHibConnectionInitializer(string serverAddress, string database) : IConfigurationInitializer
{
    public void Initialize(Configuration cfg)
    {
        cfg.Configure();
        cfg.Properties["connection.connection_string"] = $"{serverAddress};Initial Catalog={database}";
    }
}
