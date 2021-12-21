using System;
using Framework.DomainDriven.BLL.Security.Test.SecurityHierarchy.Domain;

namespace Framework.DomainDriven.BLL.Security.Test.SecurityHierarchy
{
    public class HierarchyDomainAncestorLogic : DefaultDomainBLLBase<TestBLLContext, PersistentDomainObjectBase, DomainObjectBase, HierarchyObjectAncestorLink, Guid, Operation>
    {
        public HierarchyDomainAncestorLogic(TestBLLContext context) : base(context)
        {
        }
    }
}
