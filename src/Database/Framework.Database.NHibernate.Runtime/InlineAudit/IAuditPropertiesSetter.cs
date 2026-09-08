namespace Framework.Database.NHibernate.InlineAudit;

public interface IAuditPropertiesSetter
{
    bool SetAuditFields(object[] state);
}
