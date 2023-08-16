using Framework.DomainDriven.BLL.Security.Test.SecurityHierarchy.Domain;
using Framework.DomainDriven.Lock;

namespace Framework.DomainDriven.BLL.Security.Test.SecurityHierarchy;

public enum NamedLockOperation
{
    [GlobalLock(typeof(HierarchyObjectAncestorLink))]
    HierarchyObjectAncestorLinkLock = 6
}
