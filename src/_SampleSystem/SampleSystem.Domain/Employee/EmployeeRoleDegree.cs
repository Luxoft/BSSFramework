using Framework.DomainDriven.BLL;
using Framework.Restriction;

namespace SampleSystem.Domain;

[BLLViewRole]
[SampleSystemViewDomainObject(SampleSystemSecurityOperationCode.EmployeeRoleDegreeView)]
[UniqueGroup]
public class EmployeeRoleDegree : BaseDirectory
{
}
