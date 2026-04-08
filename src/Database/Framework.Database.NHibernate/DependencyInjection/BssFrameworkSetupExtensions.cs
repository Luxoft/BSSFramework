using Framework.Infrastructure.DependencyInjection;

namespace Framework.Database.NHibernate.DependencyInjection;

public static class BssFrameworkSetupExtensions
{
    public static TSelf AddNHibernate<TSelf>(
        this IBssFrameworkSetup<TSelf> settings,
        Action<INHibernateSetup> setupAction)
        where TSelf : IBssFrameworkSetup<TSelf> =>

        settings.AddExtensions(new BssFrameworkExtension(services => services.AddNHibernate(setupAction)));
}
