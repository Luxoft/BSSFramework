using Framework.DomainDriven.BLL.Security.Lock;
using Framework.DomainDriven.BLL.Security.Test.SecurityHierarchy.Domain;

namespace Framework.DomainDriven.BLL.Security.Test.SecurityHierarchy
{
    public enum NamedLockOperation
    {
        [GlobalLock(typeof(HierarchyObjectAncestorLink))]
        HierarchyObjectAncestorLinkLock = 6
    }
}