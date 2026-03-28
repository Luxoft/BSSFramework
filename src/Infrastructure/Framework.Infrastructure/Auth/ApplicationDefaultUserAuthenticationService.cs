using Framework.Application.Auth;
using Framework.Core.Auth;

using Microsoft.AspNetCore.Http;

namespace Framework.Infrastructure.Auth;

public class ApplicationDefaultUserAuthenticationService(
    IHttpContextAccessor httpContextAccessor,
    ApplicationDefaultUserAuthenticationServiceSettings settings)
    : IDefaultUserAuthenticationService
{
    public string GetUserName() => httpContextAccessor.HttpContext?.User.Identity?.Name ?? settings.UserName;
}
