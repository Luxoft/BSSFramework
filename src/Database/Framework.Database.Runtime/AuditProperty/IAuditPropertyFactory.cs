namespace Framework.Database.AuditProperty;

public interface IAuditPropertyFactory
{
    AuditPropertyPair GetCreateAuditProperty();

    AuditPropertyPair GetModifyAuditProperty();
}
