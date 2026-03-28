using Framework.Core.Auth;

namespace Framework.Database.NHibernate.Audit;

public class AuditRevisionUserAuthenticationService(IDefaultUserAuthenticationService defaultUserAuthenticationService) : IAuditRevisionUserAuthenticationService
{
    public string GetUserName() => defaultUserAuthenticationService.GetUserName();
}
