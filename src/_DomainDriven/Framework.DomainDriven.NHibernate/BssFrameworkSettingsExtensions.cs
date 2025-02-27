using Framework.DomainDriven.Setup;

namespace Framework.DomainDriven.NHibernate;

public static class BssFrameworkSettingsExtensions
{
    public static TSelf AddNHibernate<TSelf>(
        this IBssFrameworkSettingsBase<TSelf> settings,
        Action<INHibernateSetupObject>? setupAction = null) =>

        settings.AddExtensions(new BssFrameworkExtension(services => services.AddNHibernate(setupAction)));
}
