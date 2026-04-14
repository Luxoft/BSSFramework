using Framework.Authorization.Domain;
using Framework.Database;
using Framework.Database.NHibernate.Mapping;

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
