using Framework.Database.AuditProperty;

namespace Framework.Database;

public interface IAuditPropertyFactory
{
    AuditPropertyPair GetCreateAuditProperty();

    AuditPropertyPair GetModifyAuditProperty();
}
