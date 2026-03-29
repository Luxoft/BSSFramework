namespace Framework.Application.Auth;

public record ApplicationDefaultCurrentUserSettings(string UserName)
{
    public static ApplicationDefaultCurrentUserSettings Default { get; } = new("system");
}
