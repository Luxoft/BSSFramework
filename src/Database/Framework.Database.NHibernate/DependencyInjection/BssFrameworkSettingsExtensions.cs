using Framework.Infrastructure.DependencyInjection;

namespace Framework.Database.NHibernate.DependencyInjection;

public static class BssFrameworkSettingsExtensions
{
    public static TSelf AddNHibernate<TSelf>(
        this IBssFrameworkBuilderBase<TSelf> settings,
        Action<INHibernateSetupObject> setupAction) =>

        settings.AddExtensions(new BssFrameworkExtension(services => services.AddNHibernate(setupAction)));
}
