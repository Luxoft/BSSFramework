using Framework.Authorization.Domain;
using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate;

namespace Framework.Authorization.Generated.DAL.NHibernate;

public class AuthorizationMappingSettings(DatabaseName databaseName, AuditDatabaseName auditDatabaseName)
    : MappingSettings<PersistentDomainObjectBase>(typeof(AuthorizationMappingSettings).Assembly, databaseName, auditDatabaseName)
{
    public AuthorizationMappingSettings()
        : this(new DatabaseName("", "auth"))
    {
    }

    public AuthorizationMappingSettings(DatabaseName databaseName)
        : this(databaseName, databaseName.ToDefaultAudit())
    {
    }
}
