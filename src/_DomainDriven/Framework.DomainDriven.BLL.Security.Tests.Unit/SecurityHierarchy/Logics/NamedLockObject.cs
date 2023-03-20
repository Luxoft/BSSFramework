using Framework.DomainDriven.BLL.Security.Lock;
using Framework.DomainDriven.BLL.Security.Test.SecurityHierarchy.Domain;

namespace Framework.DomainDriven.BLL.Security.Test.SecurityHierarchy;

public class NamedLockObject : PersistentDomainObjectBase, INamedLock<NamedLockOperation>
{
    public NamedLockOperation LockOperation { get; set; }
}
