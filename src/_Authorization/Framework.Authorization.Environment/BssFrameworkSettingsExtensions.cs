using Framework.DomainDriven.Setup;

namespace Framework.Authorization.Environment;

public static class BssFrameworkSettingsExtensions
{
    /// <summary>
    /// Must be called AFTER 'AddSecuritySystem'
    /// </summary>
    /// <typeparam name="TSelf"></typeparam>
    /// <param name="settings"></param>
    /// /// <param name="setup"></param>
    /// <returns></returns>
    public static TSelf AddAuthorizationSystem<TSelf>(
        this IBssFrameworkSettingsBase<TSelf> settings,
        Action<IAuthorizationSystemSettings>? setup = null) =>

        settings.AddExtensions(new BssFrameworkExtension(services => services.AddAuthorizationSystem(setup)));

}
