using NHibernate.Envers.Configuration;
using NHibernate.Envers.Query.Property;

namespace Framework.Database.NHibernate.Envers;

public class RevisionNumberFieldPropertyName : IPropertyNameGetter
{
    public string Get(AuditConfiguration auditCfg)
    {
        return "id";
    }
}
