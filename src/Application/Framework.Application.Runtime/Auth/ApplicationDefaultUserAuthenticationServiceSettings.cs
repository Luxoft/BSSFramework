namespace Framework.Application.Auth;

public record ApplicationDefaultUserAuthenticationServiceSettings(string UserName)
{
    public static ApplicationDefaultUserAuthenticationServiceSettings Default { get; } = new("system");
}
