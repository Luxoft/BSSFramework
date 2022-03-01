using Framework.DomainDriven.BLL;

namespace WorkflowSampleSystem.Domain
{
    [BLLRole]
    public class NamedLock : AuditPersistentDomainObjectBase, Framework.DomainDriven.BLL.Security.Lock.INamedLock<NamedLockOperation>
    {
        private NamedLockOperation lockOperation;

        public NamedLock()
        {

        }

        public virtual NamedLockOperation LockOperation
        {
            get { return this.lockOperation; }
            set { this.lockOperation = value; }
        }
    }
}
