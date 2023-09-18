using Framework.DomainDriven.BLL.Security.Test.SecurityHierarchy.Domain;

namespace Framework.DomainDriven.BLL.Security.Test.SecurityHierarchy;

public class NamedLockLogic : DefaultDomainBLLBase<TestBllContext, PersistentDomainObjectBase, NamedLockObject, Guid, Operation>
{
    public NamedLockLogic(TestBllContext context)
            : base(context)
    {
    }
}
