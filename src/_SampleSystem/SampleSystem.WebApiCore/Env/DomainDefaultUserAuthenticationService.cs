namespace SampleSystem.WebApiCore.Env;

public class DomainDefaultUserAuthenticationService : IDefaultUserAuthenticationService
{
    public virtual string GetUserName() => $"{System.Environment.UserDomainName}\\{System.Environment.UserName}";
}
