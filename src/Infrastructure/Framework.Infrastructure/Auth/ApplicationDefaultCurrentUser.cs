using Anch.Core.Auth;

using Framework.Application.Auth;

using Microsoft.AspNetCore.Http;

namespace Framework.Infrastructure.Auth;

public class ApplicationDefaultCurrentUser(IHttpContextAccessor httpContextAccessor, ApplicationDefaultCurrentUserSettings settings)
    : ICurrentUser
{
    public string Name => httpContextAccessor.HttpContext?.User.Identity?.Name ?? settings.UserName;
}
