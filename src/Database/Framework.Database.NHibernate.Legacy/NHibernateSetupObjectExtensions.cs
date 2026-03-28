using Framework.Database.NHibernate.DependencyInjection;

namespace Framework.Database.NHibernate;

public static class NHibernateSetupObjectExtensions
{
    public static INHibernateSetupObject AddLegacyDatabaseSettings(this INHibernateSetupObject setupObject)
    {
        return setupObject.AddExtension(new NHibernateSetupObjectExtension(services => services.AddLegacyDatabaseSettings()));
    }
}
