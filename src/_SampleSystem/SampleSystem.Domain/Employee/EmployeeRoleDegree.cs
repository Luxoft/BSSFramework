using Framework.BLL.Domain.ServiceRole;
using Framework.Restriction;

namespace SampleSystem.Domain.Employee;

[BLLViewRole]
[UniqueGroup]
public class EmployeeRoleDegree : BaseDirectory;
