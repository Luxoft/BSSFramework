using Framework.DomainDriven.DAL.Revisions;
using Framework.Persistent;

namespace SampleSystem.AuditDomain
{
    public abstract class SystemAuditPersistentDomainObjectBase : IIdentityObject<Guid>
    {
        private AuditIdentifier identifier;

        private SampleSystemAuditRevisionEntity revision;

        private Guid id;

        private AuditRevisionType revType;

        private string modifiedBy;

        private DateTime? modifyDate;

        protected SystemAuditPersistentDomainObjectBase()
        {
        }

        public virtual AuditIdentifier Identifier
        {
            get => this.identifier;
            protected internal set => this.identifier = value;
        }


        public virtual SampleSystemAuditRevisionEntity Revision => this.revision;

        public virtual Guid Id => this.id;


        public virtual AuditRevisionType RevType => this.revType;

        public virtual string ModifiedBy => this.modifiedBy;

        public virtual DateTime? ModifyDate => this.modifyDate;
    }
}
