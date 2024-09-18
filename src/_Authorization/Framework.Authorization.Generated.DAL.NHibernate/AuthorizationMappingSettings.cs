using Framework.Authorization.Domain;
using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate;

namespace Framework.Authorization.Generated.DAL.NHibernate;

public record AuthorizationMappingSettings(DatabaseName DatabaseName, AuditDatabaseName? AuditDatabaseName)
    : MappingSettings<PersistentDomainObjectBase>(typeof(AuthorizationMappingSettings).Assembly, DatabaseName, AuditDatabaseName)
{
    public AuthorizationMappingSettings(DatabaseName databaseName)
        : this(databaseName, databaseName.ToDefaultAudit())
    {
    }

    public AuthorizationMappingSettings()
        : this(new DatabaseName("", "auth"))
    {
    }
}
