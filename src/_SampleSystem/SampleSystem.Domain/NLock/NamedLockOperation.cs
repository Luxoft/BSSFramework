
using Framework.DomainDriven.Lock;

namespace SampleSystem.Domain;

public enum NamedLockOperation
{
    [GlobalLock(typeof(BusinessUnitAncestorLink))]
    BusinessUnitAncestorLock,

    [GlobalLockAttribute(typeof(ManagementUnitAncestorLink))]
    ManagementUnitAncestorLock,

    [GlobalLockAttribute(typeof(LocationAncestorLink))]
    LocationAncestorLock,
}
