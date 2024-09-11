#nullable enable

using NHibernate.Cfg;

namespace Framework.DomainDriven.NHibernate;

public interface IConfigurationInitializer
{
    void Initialize(Configuration cfg);
}
