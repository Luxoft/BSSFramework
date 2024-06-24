
using Framework.DomainDriven.Lock;

namespace SampleSystem.Domain;

public static class SampleSystemNamedLock
{
    public static NamedLock BusinessUnitAncestorLock { get; } =
        new(nameof(BusinessUnitAncestorLock), typeof(BusinessUnitAncestorLink));

    public static NamedLock ManagementUnitAncestorLock { get; } =
        new(nameof(ManagementUnitAncestorLock), typeof(ManagementUnitAncestorLink));

    public static NamedLock LocationAncestorLock { get; } =
        new(nameof(LocationAncestorLock), typeof(LocationAncestorLink));
}
