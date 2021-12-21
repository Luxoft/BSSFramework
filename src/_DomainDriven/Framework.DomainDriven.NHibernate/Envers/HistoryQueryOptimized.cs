using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Envers.Configuration;
using NHibernate.Envers.Entities.Mapper.Relation.Query;
using NHibernate.Envers.Query.Impl;
using NHibernate.Envers.Reader;

namespace NHibernate.Envers.Patch
{
    public class HistoryQueryOptimized<TEntity, TRevisionEntity, TIdentity> : AbstractRevisionsQuery<IIdentityRevisionEntityInfo<TRevisionEntity, TIdentity>>
    {
        private readonly AuditConfiguration verCfg;

        public HistoryQueryOptimized(
            AuditConfiguration auditConfiguration,
            IAuditReaderImplementor versionsReader,
            bool includesDeletations)
            : base(auditConfiguration, versionsReader, includesDeletations, typeof(TEntity).FullName)
        {
            this.verCfg = auditConfiguration;
        }

        public override IEnumerable<IIdentityRevisionEntityInfo<TRevisionEntity, TIdentity>> Results()
        {
            var auditEntitiesConfiguration = this.AuditConfiguration.AuditEntCfg;
            /*
            The query that should be executed in the versions table:
            SELECT e FROM ent_ver e, rev_entity r WHERE
              e.revision_type != DEL (if selectDeletedEntities == false) AND
              e.revision = r.revision AND
              (all specified conditions, transformed, on the "e" entity)
              ORDER BY e.revision ASC (unless another order is specified)
             */
            this.SetIncludeDeletationClause();
            this.AddCriterions();
            this.AddOrders();
            this.QueryBuilder.AddFrom(auditEntitiesConfiguration.RevisionInfoEntityFullClassName(), QueryConstants.RevisionAlias, true);
            this.QueryBuilder.RootParameters.AddWhere(auditEntitiesConfiguration.RevisionNumberPath, true, "=", QueryConstants.RevisionAlias + ".id", false);

            var revisionTypePropertyName = auditEntitiesConfiguration.RevisionTypePropName;

            return from resultRow in this.BuildAndExecuteQuery<object[]>()
                   let versionsEntity = (IDictionary)resultRow[0]
                   let revisionData = (TRevisionEntity)resultRow[1]
                   let revision = this.GetRevisionNumberFromDynamicEntity(versionsEntity)
                   let identityObject = this.GetIdentityValue(this.EntityName, versionsEntity)
                   select new IdentityRevisionEntityInfo<TRevisionEntity, TIdentity>(
                                                                                  revisionData,
                                                                                  (TIdentity)identityObject,
                                                                                  (RevisionType)versionsEntity[revisionTypePropertyName]);
        }

        public object GetIdentityValue(string entityName, IDictionary versionsEntity)
        {
            const string typeKey = "$type$";

            if (versionsEntity == null)
            {
                return null;
            }

            if (versionsEntity.Contains(typeKey))
            {
                entityName = this.verCfg.EntCfg.GetEntityNameForVersionsEntityName((string)versionsEntity[typeKey]);
            }

            // First mapping the primary key
            var idMapper = this.verCfg.EntCfg[entityName].IdMapper;
            var originalId = (IDictionary)versionsEntity[this.verCfg.AuditEntCfg.OriginalIdPropName];

            var primaryKey = idMapper.MapToIdFromMap(originalId);
            return primaryKey;
        }
    }
}
