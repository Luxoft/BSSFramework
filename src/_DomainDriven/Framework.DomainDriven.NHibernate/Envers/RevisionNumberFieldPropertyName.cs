using NHibernate.Envers.Configuration;
using NHibernate.Envers.Query.Property;

namespace NHibernate.Envers.Patch;

public class RevisionNumberFieldPropertyName : IPropertyNameGetter
{
    public string Get(AuditConfiguration auditCfg)
    {
        return "id";
    }
}
