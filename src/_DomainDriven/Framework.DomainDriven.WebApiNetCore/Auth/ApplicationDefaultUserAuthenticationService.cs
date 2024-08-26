using Framework.DomainDriven.Auth;
using Framework.DomainDriven.NHibernate.Audit;

using Microsoft.AspNetCore.Http;

namespace Framework.DomainDriven.WebApiNetCore.Auth;

public class ApplicationDefaultUserAuthenticationService(
    IHttpContextAccessor httpContextAccessor,
    IApplicationDefaultUserAuthenticationServiceSettings settings)
    : IDefaultUserAuthenticationService, IAuditRevisionUserAuthenticationService
{
    public string GetUserName() => httpContextAccessor.HttpContext?.User?.Identity?.Name ?? settings.DefaultValue;
}
