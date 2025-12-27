namespace Framework.DomainDriven.Auth;

public record ApplicationDefaultUserAuthenticationServiceSettings(string DefaultValue = "system") : IApplicationDefaultUserAuthenticationServiceSettings;
