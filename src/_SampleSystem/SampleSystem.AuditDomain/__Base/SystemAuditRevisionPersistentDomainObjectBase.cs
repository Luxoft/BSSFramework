using Framework.Persistent;

namespace SampleSystem.AuditDomain
{
    public abstract class SystemAuditRevisionPersistentDomainObjectBase : IIdentityObject<long>
    {
        private long id;

        protected SystemAuditRevisionPersistentDomainObjectBase()
        {
        }

        public virtual long Id => this.id;
    }
}
