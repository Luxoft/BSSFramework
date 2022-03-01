using Framework.DomainDriven.BLL;
using Framework.Restriction;

namespace WorkflowSampleSystem.Domain
{
    [BLLViewRole]
    [WorkflowSampleSystemViewDomainObject(WorkflowSampleSystemSecurityOperationCode.EmployeeRoleDegreeView)]
    [UniqueGroup]
    public class EmployeeRoleDegree : BaseDirectory
    {
    }
}
