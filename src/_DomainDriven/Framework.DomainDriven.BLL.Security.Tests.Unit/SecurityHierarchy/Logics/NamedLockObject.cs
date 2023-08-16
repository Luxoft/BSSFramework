using Framework.DomainDriven.BLL.Security.Test.SecurityHierarchy.Domain;
using Framework.DomainDriven.Lock;

namespace Framework.DomainDriven.BLL.Security.Test.SecurityHierarchy;

public class NamedLockObject : PersistentDomainObjectBase, INamedLock<NamedLockOperation>
{
    public NamedLockOperation LockOperation { get; set; }
}
