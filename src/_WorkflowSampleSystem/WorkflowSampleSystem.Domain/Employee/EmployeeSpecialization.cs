using Framework.DomainDriven.BLL;
using Framework.Restriction;

namespace WorkflowSampleSystem.Domain
{
    [BLLViewRole]
    [WorkflowSampleSystemViewDomainObject(WorkflowSampleSystemSecurityOperationCode.EmployeeSpecializationView)]
    [UniqueGroup]
    public class EmployeeSpecialization : BaseDirectory
    {
    }
}
