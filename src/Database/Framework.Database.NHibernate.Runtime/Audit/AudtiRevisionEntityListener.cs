using Framework.Database.NHibernate.DAL.Revisions;

using NHibernate.Envers;

namespace Framework.Database.NHibernate.Audit;

/// <summary>
/// Concrete implement for revision object type AuditRevisionEntity
/// </summary>
public class AuditRevisionEntityListener<TAuditRevisionEntity>(IAuditRevisionUserAuthenticationService auditRevisionUserAuthenticationService)
    : RevisionEntityListener<TAuditRevisionEntity>(auditRevisionUserAuthenticationService)
    where TAuditRevisionEntity : AuditRevisionEntity
{
    protected override void ProcessNewRevision(TAuditRevisionEntity revisionEntity) => this.SetAuthor(revisionEntity);

    protected override void ProcessEntityChanged(Type entityClass, object entityId, RevisionType revisionType, TAuditRevisionEntity revisionEntity) => this.SetAuthor(revisionEntity);

    private void SetAuthor(TAuditRevisionEntity revisionEntity) => revisionEntity.Author = this.AuditRevisionUserAuthenticationService.GetUserName();
}
