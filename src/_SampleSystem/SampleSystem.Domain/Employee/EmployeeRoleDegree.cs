using Framework.DomainDriven.BLL;
using Framework.Restriction;
using Framework.Security;

namespace SampleSystem.Domain;

[BLLViewRole]
[UniqueGroup]
[ViewDomainObject(typeof(SampleSystemSecurityOperation), nameof(SampleSystemSecurityOperation.EmployeeRoleDegreeView))]
public class EmployeeRoleDegree : BaseDirectory
{
}
