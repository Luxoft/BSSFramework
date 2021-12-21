using System;
using Framework.DomainDriven.BLL.Security.Test.SecurityHierarchy.Domain;

namespace Framework.DomainDriven.BLL.Security.Test.SecurityHierarchy
{
    public class NamedLockLogic : DefaultDomainBLLBase<TestBLLContext, PersistentDomainObjectBase, DomainObjectBase, NamedLockObject, Guid, Operation>
    {
        public NamedLockLogic(TestBLLContext context)
            : base(context)
        {
        }
    }
}