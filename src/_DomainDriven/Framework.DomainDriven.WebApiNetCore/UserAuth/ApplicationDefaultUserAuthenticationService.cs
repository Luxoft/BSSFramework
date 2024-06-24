using Framework.Core.Services;
using Framework.DomainDriven.NHibernate.Audit;

using Microsoft.AspNetCore.Http;

namespace Framework.DomainDriven.WebApiNetCore;

public class ApplicationDefaultUserAuthenticationService : DomainDefaultUserAuthenticationService, IAuditRevisionUserAuthenticationService
{
    private readonly IHttpContextAccessor httpContextAccessor;


    public ApplicationDefaultUserAuthenticationService(IHttpContextAccessor httpContextAccessor) => this.httpContextAccessor = httpContextAccessor;


    public override string GetUserName() => this.httpContextAccessor.HttpContext?.User?.Identity?.Name ?? base.GetUserName();
}
