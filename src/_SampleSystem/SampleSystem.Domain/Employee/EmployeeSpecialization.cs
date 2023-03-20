using Framework.DomainDriven.BLL;
using Framework.Restriction;

namespace SampleSystem.Domain;

[BLLViewRole]
[SampleSystemViewDomainObject(SampleSystemSecurityOperationCode.EmployeeSpecializationView)]
[UniqueGroup]
public class EmployeeSpecialization : BaseDirectory
{
}
