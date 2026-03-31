using Framework.BLL.Domain.ServiceRole;
using Framework.DomainDriven.BLL;
using Framework.Restriction;

namespace SampleSystem.Domain;

[BLLViewRole]
[UniqueGroup]
public class EmployeeRole : BaseDirectory
{
}
