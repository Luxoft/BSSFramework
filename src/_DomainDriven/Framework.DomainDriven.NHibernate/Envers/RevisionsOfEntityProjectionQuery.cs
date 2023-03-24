using System;
using System.Collections;
using NHibernate.Envers.Configuration;
using NHibernate.Envers.Entities.Mapper.Relation.Query;
using NHibernate.Envers.Query.Impl;
using NHibernate.Envers.Reader;
using NHibernate.Proxy;

namespace NHibernate.Envers.Patch;

public class RevisionsOfEntityProjectionQuery<T> : RevisionsOfEntityQuery
{
    private readonly bool selectEntitiesOnly;
    private readonly bool selectDeletedEntities;

    public RevisionsOfEntityProjectionQuery(
            AuditConfiguration verCfg,
            IAuditReaderImplementor versionsReader,
            bool selectEntitiesOnly,
            bool selectDeletedEntities)
            : base(verCfg, versionsReader, typeof(T), selectEntitiesOnly, selectDeletedEntities)
    {
        this.selectEntitiesOnly = selectEntitiesOnly;
        this.selectDeletedEntities = selectDeletedEntities;
    }

    public RevisionsOfEntityProjectionQuery(
            AuditConfiguration verCfg,
            IAuditReaderImplementor versionsReader,
            string entityName,
            bool selectEntitiesOnly,
            bool selectDeletedEntities)
            : base(verCfg, versionsReader, entityName, selectEntitiesOnly, selectDeletedEntities)
    {
        this.selectEntitiesOnly = selectEntitiesOnly;
        this.selectDeletedEntities = selectDeletedEntities;
    }

    private long GetRevisionNumber(IDictionary versionsEntity)
    {
        var verEntCfg = this.VerCfg.AuditEntCfg;
        var originalId = verEntCfg.OriginalIdPropName;
        var revisionPropertyName = verEntCfg.RevisionFieldName;
        var revisionInfoObject = ((IDictionary)versionsEntity[originalId])[revisionPropertyName];
        var proxy = revisionInfoObject as INHibernateProxy;

        return proxy != null ? Convert.ToInt64(proxy.HibernateLazyInitializer.Identifier) : this.VerCfg.RevisionInfoNumberReader.RevisionNumber(revisionInfoObject);
    }

    protected override void FillResult(IList result)
    {
        var verEntCfg = this.VerCfg.AuditEntCfg;

        /*
        The query that should be executed in the versions table:
        SELECT e (unless another projection is specified) FROM ent_ver e, rev_entity r WHERE
          e.revision_type != DEL (if selectDeletedEntities == false) AND
          e.revision = r.revision AND
          (all specified conditions, transformed, on the "e" entity)
          ORDER BY e.revision ASC (unless another order or projection is specified)
         */
        if (!this.selectDeletedEntities)
        {
            // e.revision_type != DEL AND
            this.QueryBuilder.RootParameters.AddWhereWithParam(verEntCfg.RevisionTypePropName, "<>", RevisionType.Deleted);
        }

        // all specified conditions, transformed
        foreach (var criterion in this.Criterions)
        {
            criterion.AddToQuery(this.VerCfg, this.VersionsReader, this.EntityName, this.QueryBuilder, this.QueryBuilder.RootParameters);
        }

        if (!this.HasProjection() && !this.HasOrder)
        {
            var revisionPropertyPath = verEntCfg.RevisionNumberPath;
            this.QueryBuilder.AddOrder(QueryConstants.ReferencedEntityAlias, revisionPropertyPath, true);
        }

        if (!this.selectEntitiesOnly)
        {
            this.QueryBuilder.AddFrom(this.VerCfg.AuditEntCfg.RevisionInfoEntityFullClassName(), QueryConstants.RevisionAlias, true);
            this.QueryBuilder.RootParameters.AddWhere(this.VerCfg.AuditEntCfg.RevisionNumberPath, true, "=", QueryConstants.RevisionAlias + ".id", false);
        }

        var internalResult = new ArrayList();
        this.BuildAndExecuteQuery(internalResult);

        var revisionTypePropertyName = verEntCfg.RevisionTypePropName;

        foreach (var resultRow in internalResult)
        {
            IDictionary versionsEntity;
            object revisionData = null;

            if (this.selectEntitiesOnly)
            {
                versionsEntity = (IDictionary)resultRow;
            }
            else
            {
                var arrayResultRow = (object[])resultRow;
                versionsEntity = (IDictionary)arrayResultRow[0];
                revisionData = arrayResultRow[1];
            }

            var revision = this.GetRevisionNumber(versionsEntity);

            var entity = this.EntityInstantiator.CreateInstanceFromVersionsEntity(this.EntityName, versionsEntity, revision);

            result.Add(
                       this.selectEntitiesOnly
                               ? entity
                               : new[] { entity, revisionData, versionsEntity[revisionTypePropertyName] });
        }
    }
}
