using Framework.DomainDriven.BLL;
using Framework.Restriction;

namespace WorkflowSampleSystem.Domain
{
    [BLLViewRole]
    [WorkflowSampleSystemViewDomainObject(WorkflowSampleSystemSecurityOperationCode.EmployeeRoleView)]
    [UniqueGroup]
    public class EmployeeRole : BaseDirectory
    {
    }
}
