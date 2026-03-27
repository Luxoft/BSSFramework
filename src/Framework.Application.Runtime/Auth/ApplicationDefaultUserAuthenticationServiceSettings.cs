namespace Framework.Application.Auth;

public record ApplicationDefaultUserAuthenticationServiceSettings(string Name)
{
    public static ApplicationDefaultUserAuthenticationServiceSettings Default { get; } = new("system");
}
