using Framework.BLL.Domain.ServiceRole;
using Framework.Restriction;

namespace SampleSystem.Domain;

[BLLViewRole]
[UniqueGroup]
public class EmployeeRole : BaseDirectory;
