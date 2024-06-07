using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL;

public partial interface IControlSettingsBLL
{
    ControlSettings GetRootControlSettingsForCurrentPrincipal(string name);

    ControlSettings GetRootControlSettings(string name, string accountName);

    public void AddChild(ControlSettings parent, ControlSettings child);
}
