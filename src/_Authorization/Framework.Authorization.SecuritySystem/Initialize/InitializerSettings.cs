namespace Framework.Authorization.SecuritySystem.Initialize;

public record InitializerSettings(
    UnexpectedAuthElementMode UnexpectedAuthElementMode = UnexpectedAuthElementMode.Remove,
    bool InitDefaultAdminRole = false);
