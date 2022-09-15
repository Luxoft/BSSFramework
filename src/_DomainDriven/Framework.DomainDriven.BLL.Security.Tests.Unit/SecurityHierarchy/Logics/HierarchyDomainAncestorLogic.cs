using System;
using Framework.DomainDriven.BLL.Security.Test.SecurityHierarchy.Domain;

namespace Framework.DomainDriven.BLL.Security.Test.SecurityHierarchy
{
    public class HierarchyDomainAncestorLogic : DefaultDomainBLLBase<TestBllContext, PersistentDomainObjectBase, DomainObjectBase, HierarchyObjectAncestorLink, Guid, Operation>
    {
        public HierarchyDomainAncestorLogic(TestBllContext context) : base(context)
        {
        }
    }
}
