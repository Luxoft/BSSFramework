using Framework.Core.Services;
using Framework.DomainDriven.Audit;

namespace Framework.DomainDriven;

public class DBSessionSettings(
    IUserAuthenticationService userAuthenticationService,
    TimeProvider timeProvider)
    : IDBSessionSettings
{
    public DBSessionMode DefaultSessionMode { get; } = DBSessionMode.Write;

    public AuditPropertyPair GetCreateAuditProperty() => AuditPropertyPair.GetCreateAuditProperty(userAuthenticationService, timeProvider);

    public AuditPropertyPair GetModifyAuditProperty() => AuditPropertyPair.GetModifyAuditProperty(userAuthenticationService, timeProvider);
}
