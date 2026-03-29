using CommonFramework.Auth;

using Framework.Database.AuditProperty;

namespace Framework.Database.NHibernate;

public class AuditPropertyFactory(
    ICurrentUser currentUser,
    TimeProvider timeProvider) : IAuditPropertyFactory
{
    public AuditPropertyPair GetCreateAuditProperty() => AuditPropertyPair.GetCreateAuditProperty(currentUser, timeProvider);

    public AuditPropertyPair GetModifyAuditProperty() => AuditPropertyPair.GetModifyAuditProperty(currentUser, timeProvider);
}
