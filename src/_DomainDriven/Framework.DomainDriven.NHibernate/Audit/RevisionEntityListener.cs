using System;

using Framework.Core.Services;

using NHibernate.Envers;

namespace Framework.DomainDriven.NHibernate.Audit
{
    /// <summary>
    /// Base Typed Implement of IEntityTrackingRevisionListener
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RevisionEntityListener<T> : IEntityTrackingRevisionListener
    {
        private readonly IUserAuthenticationService userAuthenticationService;

        protected RevisionEntityListener(IUserAuthenticationService userAuthenticationService)
        {
            this.userAuthenticationService = userAuthenticationService;
        }

        protected IUserAuthenticationService UserAuthenticationService => this.userAuthenticationService;

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
}
