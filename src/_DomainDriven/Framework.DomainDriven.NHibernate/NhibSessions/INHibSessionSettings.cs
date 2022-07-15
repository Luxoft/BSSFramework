using Framework.DomainDriven.Audit;
using Framework.DomainDriven.BLL;

namespace Framework.DomainDriven.NHibernate;

public interface INHibSessionSettings
{
    DBSessionMode DefaultSessionMode { get; }

    AuditPropertyPair GetCreateAuditProperty();

    AuditPropertyPair GetModifyAuditProperty();
}
