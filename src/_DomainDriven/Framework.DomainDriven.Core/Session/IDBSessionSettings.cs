using Framework.DomainDriven.Audit;

namespace Framework.DomainDriven;

public interface IDBSessionSettings
{
    DBSessionMode DefaultSessionMode { get; }

    AuditPropertyPair GetCreateAuditProperty();

    AuditPropertyPair GetModifyAuditProperty();
}
