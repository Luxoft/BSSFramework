namespace Framework.DomainDriven.WebApiNetCore.Auth;

public record ApplicationDefaultUserAuthenticationServiceSettings(string DefaultValue = "system") : IApplicationDefaultUserAuthenticationServiceSettings;
