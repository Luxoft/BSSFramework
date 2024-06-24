using Framework.DomainDriven.Lock;
using Framework.DomainDriven.BLL.Security.Test.SecurityHierarchy.Domain;

namespace Framework.DomainDriven.BLL.Security.Test.SecurityHierarchy;

public static class TestNamedLock
{
    public static NamedLock HierarchyObjectAncestorLinkLock { get; } = new(
        nameof(HierarchyObjectAncestorLinkLock),
        typeof(HierarchyObjectAncestorLink));
}
