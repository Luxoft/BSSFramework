using Framework.DomainDriven.Audit;

namespace Framework.DomainDriven.NHibernate;

public interface INHibSessionSetup
{
    DBSessionMode DefaultSessionMode { get; }

    AuditPropertyPair GetCreateAuditProperty();

    AuditPropertyPair GetModifyAuditProperty();
}
