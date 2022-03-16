using System;

using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate;
using Framework.Attachments.Domain;

namespace Framework.Attachments.Generated.DAL.NHibernate
{
    public class AttachmentsMappingSettings : MappingSettings<PersistentDomainObjectBase>
    {
        public AttachmentsMappingSettings(DatabaseName databaseName)
            : base(typeof(AttachmentsMappingSettings).Assembly, databaseName)
        {
        }

        /// <summary>
        ///     Дефолтные настроки базы данных.  Таблицы имеют схему configuration
        /// </summary>
        public static IMappingSettings CreateWithoutAudit(string mainDatabaseName) =>
            new AttachmentsMappingSettings(CreateDatabase(mainDatabaseName));

        private static DatabaseName CreateDatabase(string mainDatabaseName) =>
            new DatabaseName(mainDatabaseName, "configuration");
    }
}
