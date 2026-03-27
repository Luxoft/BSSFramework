using Framework.Application.Session;
using Framework.Database.AuditProperty;

using SecuritySystem.Services;

namespace Framework.Database.NHibernate;

public class DBSessionSettings(
    IRawUserAuthenticationService userAuthenticationService,
    TimeProvider timeProvider)
    : IdbSessionSettings
{
    public DBSessionMode DefaultSessionMode { get; } = DBSessionMode.Write;

    public AuditPropertyPair GetCreateAuditProperty() => AuditPropertyPair.GetCreateAuditProperty(userAuthenticationService, timeProvider);

    public AuditPropertyPair GetModifyAuditProperty() => AuditPropertyPair.GetModifyAuditProperty(userAuthenticationService, timeProvider);
}
