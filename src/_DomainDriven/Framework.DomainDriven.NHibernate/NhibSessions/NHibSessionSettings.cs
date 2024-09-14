using Framework.Core.Services;
using Framework.DomainDriven.Audit;

namespace Framework.DomainDriven.NHibernate;

public class NHibSessionSettings(
    IUserAuthenticationService userAuthenticationService,
    TimeProvider timeProvider)
    : INHibSessionSetup
{
    public DBSessionMode DefaultSessionMode { get; } = DBSessionMode.Write;

    public AuditPropertyPair GetCreateAuditProperty() => AuditPropertyPair.GetCreateAuditProperty(userAuthenticationService, timeProvider);

    public AuditPropertyPair GetModifyAuditProperty() => AuditPropertyPair.GetModifyAuditProperty(userAuthenticationService, timeProvider);
}
