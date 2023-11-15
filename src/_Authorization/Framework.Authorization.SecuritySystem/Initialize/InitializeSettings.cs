namespace Framework.Authorization.SecuritySystem.Initialize;

public record InitializeSettings(
    UnexpectedAuthElementMode UnexpectedAuthElementMode = UnexpectedAuthElementMode.Remove,
    bool InitDefaultAdminRole = false);
