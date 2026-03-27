using SecuritySystem.Services;

namespace Framework.Database.AuditProperty;

public class AuditPropertyFactory(
    IRawUserAuthenticationService userAuthenticationService,
    TimeProvider timeProvider) : IAuditPropertyFactory
{
    public AuditPropertyPair GetCreateAuditProperty() => AuditPropertyPair.GetCreateAuditProperty(userAuthenticationService, timeProvider);

    public AuditPropertyPair GetModifyAuditProperty() => AuditPropertyPair.GetModifyAuditProperty(userAuthenticationService, timeProvider);
}
