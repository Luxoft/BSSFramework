using NHibernate.Envers.Configuration;
using NHibernate.Envers.Query.Property;

namespace Framework.Database.NHibernate.EnversAudit;

public class RevisionNumberFieldPropertyName : IPropertyNameGetter
{
    public string Get(AuditConfiguration auditCfg) => "id";
}
