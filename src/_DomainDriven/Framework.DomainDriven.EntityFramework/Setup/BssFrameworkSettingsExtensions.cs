using Framework.DomainDriven.Setup;

namespace Framework.DomainDriven.EntityFramework;

public static class BssFrameworkSettingsExtensions
{
    public static TSelf AddEntityFramework<TSelf>(
        this IBssFrameworkSettingsBase<TSelf> settings,
        Action<IEntityFrameworkSetupObject> setupAction) =>

        settings.AddExtensions(new BssFrameworkExtension(services => services.AddEntityFramework(setupAction)));
}
