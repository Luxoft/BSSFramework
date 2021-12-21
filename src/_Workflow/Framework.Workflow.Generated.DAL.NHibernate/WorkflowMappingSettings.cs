using System;

using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate;
using Framework.Workflow.Domain;

namespace Framework.Workflow.Generated.DAL.NHibernate
{
    public class WorkflowMappingSettings : MappingSettings<PersistentDomainObjectBase>
    {
        public WorkflowMappingSettings(DatabaseName databaseName)
            : base(typeof(WorkflowMappingSettings).Assembly, databaseName)
        {
        }

        public WorkflowMappingSettings(DatabaseName databaseName, AuditDatabaseName auditDatabaseName)
            : base(typeof(WorkflowMappingSettings).Assembly, databaseName, auditDatabaseName)
        {
        }

        /// <summary>
        ///     Дефолтные настроки базы данных
        ///     Таблицы имеют схему workflow, ревизии хранятся в схеме appAudit
        /// </summary>
        public static WorkflowMappingSettings CreateDefaultAudit(string mainDatabaseName)
        {
            var databaseName = WorkflowMappingSettings.CreateDatabase(mainDatabaseName);

            return new WorkflowMappingSettings(databaseName, databaseName.ToDefaultAudit());
        }

        [Obsolete("Use constructor", true)]
        public static IMappingSettings CreateWithoutAudit(DatabaseName databaseName) =>
            new WorkflowMappingSettings(databaseName);

        /// <summary>
        ///     Дефолтные настроки базы данных.  Таблицы имеют схему workflow
        /// </summary>
        public static IMappingSettings CreateWithoutAudit(string mainDatabaseName) =>
            new WorkflowMappingSettings(CreateDatabase(mainDatabaseName));

        private static DatabaseName CreateDatabase(string mainDatabaseName) =>
            new DatabaseName(mainDatabaseName, "workflow");
    }
}
