using JetBrains.Annotations;

using NHibernate.Envers;

namespace Framework.DomainDriven.NHibernate.Audit;

/// <summary>
/// Base Typed Implement of IEntityTrackingRevisionListener
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class RevisionEntityListener<T> : IEntityTrackingRevisionListener
{
    protected RevisionEntityListener([NotNull] IAuditRevisionUserAuthenticationService auditRevisionUserAuthenticationService)
    {
        this.AuditRevisionUserAuthenticationService = auditRevisionUserAuthenticationService ?? throw new ArgumentNullException(nameof(auditRevisionUserAuthenticationService));
    }

    protected IAuditRevisionUserAuthenticationService AuditRevisionUserAuthenticationService {get; }

    public void NewRevision(object revisionEntity)
    {
        this.ProcessNewRevision((T)revisionEntity);
    }

    public void EntityChanged(Type entityClass, string entityName, object entityId, RevisionType revisionType,
                              object revisionEntity)
    {
        this.ProcessEntityChanged(entityClass, entityId, revisionType, (T)revisionEntity);
    }

    protected abstract void ProcessNewRevision(T revisionEntity);

    protected abstract void ProcessEntityChanged(Type entityClass, object entityId, RevisionType revisionType,
                                                 T revisionEntity);
}
