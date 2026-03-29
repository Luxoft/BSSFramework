using CommonFramework.Auth;

namespace Framework.Database.AuditProperty;

public class AuditPropertyFactory(
    ICurrentUser currentUser,
    TimeProvider timeProvider) : IAuditPropertyFactory
{
    public AuditPropertyPair GetCreateAuditProperty() => AuditPropertyPair.GetCreateAuditProperty(currentUser, timeProvider);

    public AuditPropertyPair GetModifyAuditProperty() => AuditPropertyPair.GetModifyAuditProperty(currentUser, timeProvider);
}
