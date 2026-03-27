using Framework.Database.AuditProperty;

namespace Framework.Database.NHibernate;

public interface IAuditPropertyFactory
{
    AuditPropertyPair GetCreateAuditProperty();

    AuditPropertyPair GetModifyAuditProperty();
}
