using NHibernate.Envers.Configuration;
using NHibernate.Envers.Query;
using NHibernate.Envers.Query.Impl;
using NHibernate.Envers.Reader;

namespace NHibernate.Envers.Patch;

public class AuditQueryCreatorPatched : AuditQueryCreator
{
    private readonly AuditConfiguration auditCfg;
    private readonly IAuditReaderImplementor auditReaderImplementor;

    public AuditQueryCreatorPatched(AuditConfiguration auditCfg, IAuditReaderImplementor auditReaderImplementor)
            : base(auditCfg, auditReaderImplementor)
    {
        this.auditCfg = auditCfg;
        this.auditReaderImplementor = auditReaderImplementor;
    }

    public IAuditQuery ForProjectingRevisionsOfEntity<T>(bool selectEntitiesOnly, bool selectDeletedEntities)
    {
        return new RevisionsOfEntityProjectionQuery<T>(this.auditCfg, this.auditReaderImplementor, selectEntitiesOnly, selectDeletedEntities);
    }

    /// <summary>
    /// without materialized entity objects
    /// </summary>
    public IEntityAuditQuery<IIdentityRevisionEntityInfo<TRevisionInfo, TIdentity>> ForHistoryOf<TEntity, TRevisionInfo, TIdentity>(bool includeDeleted)
    {
        return new HistoryQueryOptimized<TEntity, TRevisionInfo, TIdentity>(this.auditCfg, this.auditReaderImplementor, includeDeleted);
    }

    public RevisionsOfEntityQuery CreateRevisionEntityQuery()
    {
        return new RevisionsOfEntityQuery(this.auditCfg, this.auditReaderImplementor, this.auditCfg.AuditEntCfg.RevisionInfoEntityFullClassName(), true, false);
    }
}
