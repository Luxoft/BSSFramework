using Framework.DomainDriven.Audit;

using SecuritySystem.Services;

namespace Framework.DomainDriven;

public class DBSessionSettings(
    IRawUserAuthenticationService userAuthenticationService,
    TimeProvider timeProvider)
    : IDBSessionSettings
{
    public DBSessionMode DefaultSessionMode { get; } = DBSessionMode.Write;

    public AuditPropertyPair GetCreateAuditProperty() => AuditPropertyPair.GetCreateAuditProperty(userAuthenticationService, timeProvider);

    public AuditPropertyPair GetModifyAuditProperty() => AuditPropertyPair.GetModifyAuditProperty(userAuthenticationService, timeProvider);
}
