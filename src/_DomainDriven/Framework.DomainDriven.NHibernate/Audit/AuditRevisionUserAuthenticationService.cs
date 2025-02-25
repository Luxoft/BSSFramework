using Framework.DomainDriven.Auth;

namespace Framework.DomainDriven.NHibernate.Audit;

public class AuditRevisionUserAuthenticationService(IDefaultUserAuthenticationService defaultUserAuthenticationService) : IAuditRevisionUserAuthenticationService
{
    public string GetUserName() => defaultUserAuthenticationService.GetUserName();
}
