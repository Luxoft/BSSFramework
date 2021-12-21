using Framework.Authorization.Domain;
using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate;

namespace Framework.Authorization.Generated.DAL.NHibernate
{
    public class AuthorizationMappingSettings : MappingSettings<PersistentDomainObjectBase>
    {
        public AuthorizationMappingSettings()
            : base(typeof(AuthorizationMappingSettings).Assembly)
        {
        }

        public AuthorizationMappingSettings(DatabaseName databaseName)
            : base(typeof(AuthorizationMappingSettings).Assembly, databaseName)
        {
        }

        public AuthorizationMappingSettings(DatabaseName databaseName, AuditDatabaseName auditDatabaseName)
            : base(typeof(AuthorizationMappingSettings).Assembly, databaseName, auditDatabaseName)
        {
        }

        /// <summary>
        ///     Дефолтный настроки базы данных
        ///     Аудит имеет схему auth, ревизии хранятся в схеме appAudit
        /// </summary>
        public static AuthorizationMappingSettings CreateDefaultAudit(string mainDatabaseName)
        {
            var databaseName = new DatabaseName(mainDatabaseName, "auth");

            return new AuthorizationMappingSettings(databaseName, databaseName.ToDefaultAudit());
        }
    }
}
