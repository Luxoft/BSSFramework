using Framework.DomainDriven.Auth;

using Microsoft.AspNetCore.Http;

namespace Framework.DomainDriven.WebApiNetCore.Auth;

public class ApplicationDefaultUserAuthenticationService(
    IHttpContextAccessor httpContextAccessor,
    IApplicationDefaultUserAuthenticationServiceSettings settings)
    : IDefaultUserAuthenticationService
{
    public string GetUserName() => httpContextAccessor.HttpContext?.User?.Identity?.Name ?? settings.DefaultValue;
}
