namespace Framework.DomainDriven.WebApiNetCore.Auth;

public class EnvironmentDefaultUserAuthenticationServiceSettings : IApplicationDefaultUserAuthenticationServiceSettings
{
    public string DefaultValue => $"{System.Environment.UserDomainName}\\{System.Environment.UserName}";
}
