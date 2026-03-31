using Framework.Infrastructure.DependencyInjection;

namespace Framework.Database.EntityFramework.Setup;

public static class BssFrameworkSettingsExtensions
{
    public static TSelf AddEntityFramework<TSelf>(
        this IBssFrameworkBuilderBase<TSelf> settings,
        Action<IEntityFrameworkSetupObject> setupAction) =>

        settings.AddExtensions(new BssFrameworkExtension(services => services.AddEntityFramework(setupAction)));
}
