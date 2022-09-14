using System;
using Framework.DomainDriven.BLL.Security.Test.SecurityHierarchy.Domain;

namespace Framework.DomainDriven.BLL.Security.Test.SecurityHierarchy
{
    public class NamedLockLogic : DefaultDomainBLLBase<TestBllContext, PersistentDomainObjectBase, DomainObjectBase, NamedLockObject, Guid, Operation>
    {
        public NamedLockLogic(TestBllContext context)
            : base(context)
        {
        }
    }
}