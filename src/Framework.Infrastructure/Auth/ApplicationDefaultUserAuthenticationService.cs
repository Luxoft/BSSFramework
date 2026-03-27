using Framework.Core.Auth;
using Framework.DomainDriven.Auth;
using Microsoft.AspNetCore.Http;

namespace Framework.Infrastructure.Auth;

public class ApplicationDefaultUserAuthenticationService(
    IHttpContextAccessor httpContextAccessor,
    IApplicationDefaultUserAuthenticationServiceSettings settings)
    : IDefaultUserAuthenticationService
{
    public string GetUserName() => httpContextAccessor.HttpContext?.User.Identity?.Name ?? settings.DefaultValue;
}
