using Framework.Database.NHibernate.DependencyInjection;

namespace Framework.Database.NHibernate;

public static class NHibernateSetupObjectExtensions
{
    public static INHibernateSetup AddLegacyDatabaseSettings(this INHibernateSetup setupObject) => setupObject.AddExtension(new NHibernateSetupExtension(services => services.AddLegacyNHibernateSettings()));
}
