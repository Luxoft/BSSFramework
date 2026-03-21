using Framework.Application.AuditProperty;

namespace Framework.Application.Session;

public interface IDBSessionSettings
{
    DBSessionMode DefaultSessionMode { get; }

    AuditPropertyPair GetCreateAuditProperty();

    AuditPropertyPair GetModifyAuditProperty();
}
