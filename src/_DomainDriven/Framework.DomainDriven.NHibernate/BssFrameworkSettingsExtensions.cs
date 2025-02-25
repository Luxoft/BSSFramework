using Framework.DomainDriven.NHibernate;
using Framework.DomainDriven.Setup;

namespace Framework.Authorization.Environment;

public static class BssFrameworkSettingsExtensions
{
    public static TSelf AddNHibernate<TSelf>(
        this IBssFrameworkSettingsBase<TSelf> settings,
        Action<INHibernateSetupObject>? setupAction = null) =>

        settings.AddExtensions(new BssFrameworkExtension(services => services.AddDatabaseSettings(setupAction)));
}
