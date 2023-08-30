
namespace SampleSystem.Domain;

public enum NamedLockOperation
{
    [Framework.DomainDriven.Lock.GlobalLockAttribute(typeof(BusinessUnitAncestorLink))]
    BusinessUnitAncestorLock,

    [Framework.DomainDriven.Lock.GlobalLockAttribute(typeof(ManagementUnitAncestorLink))]
    ManagementUnitAncestorLock,

    [Framework.DomainDriven.Lock.GlobalLockAttribute(typeof(LocationAncestorLink))]
    LocationAncestorLock,
}
