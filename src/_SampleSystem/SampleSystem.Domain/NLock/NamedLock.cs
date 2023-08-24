using Framework.DomainDriven.BLL;

namespace SampleSystem.Domain;

[BLLRole]
public class NamedLock : AuditPersistentDomainObjectBase, Framework.DomainDriven.Lock.INamedLock<NamedLockOperation>
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
