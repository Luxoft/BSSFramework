using Framework.DomainDriven.BLL;
using Framework.Restriction;

namespace SampleSystem.Domain;

[BLLViewRole]
[SampleSystemViewDomainObject(SampleSystemSecurityOperationCode.EmployeeRoleView)]
[UniqueGroup]
public class EmployeeRole : BaseDirectory
{
}
