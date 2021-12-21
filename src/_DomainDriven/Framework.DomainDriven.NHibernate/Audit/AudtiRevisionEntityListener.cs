using System;

using Framework.Core;
using Framework.Core.Services;
using Framework.DomainDriven.DAL.Revisions;

using NHibernate.Envers;

namespace Framework.DomainDriven.NHibernate.Audit
{
    /// <summary>
    /// Concrete implement for revision object type AuditRevisionEntity
    /// </summary>
    public class AuditRevisionEntityListener<TAuditRevisionEntity> : RevisionEntityListener<TAuditRevisionEntity>
        where TAuditRevisionEntity : AuditRevisionEntity
    {
        public AuditRevisionEntityListener(IUserAuthenticationService userAuthenticationService)
            : base(userAuthenticationService)
        {
        }

        protected override void ProcessNewRevision(TAuditRevisionEntity revisionEntity)
        {
            this.SetAuthor(revisionEntity);
        }

        protected override void ProcessEntityChanged(Type entityClass, object entityId, RevisionType revisionType, TAuditRevisionEntity revisionEntity)
        {
            this.SetAuthor(revisionEntity);
        }

        private void SetAuthor(TAuditRevisionEntity revisionEntity)
        {
            revisionEntity.Author = this.UserAuthenticationService.GetUserName();
        }
    }
}
