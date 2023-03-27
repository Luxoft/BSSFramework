
namespace SampleSystem.Domain;

public enum NamedLockOperation
{
    [Framework.DomainDriven.BLL.Security.Lock.GlobalLockAttribute(typeof(BusinessUnitAncestorLink))]
    BusinessUnitAncestorLock,

    [Framework.DomainDriven.BLL.Security.Lock.GlobalLockAttribute(typeof(ManagementUnitAncestorLink))]
    ManagementUnitAncestorLock,

    [Framework.DomainDriven.BLL.Security.Lock.GlobalLockAttribute(typeof(LocationAncestorLink))]
    LocationAncestorLock,
}
