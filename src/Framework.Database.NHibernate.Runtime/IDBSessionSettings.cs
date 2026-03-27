using Framework.Application.Session;
using Framework.Database.AuditProperty;

namespace Framework.Database.NHibernate;

public interface IdbSessionSettings
{
    DBSessionMode DefaultSessionMode { get; }

    AuditPropertyPair GetCreateAuditProperty();

    AuditPropertyPair GetModifyAuditProperty();
}
