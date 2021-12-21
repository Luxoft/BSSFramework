using Framework.DomainDriven;
using Framework.DomainDriven.BLL.Security;
using Framework.Persistent;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.BLL
{
    public interface ITaskInstanceContract : IDomainObjectContract<TaskInstance>
    {
        //[BLLSecurityMode(BLLSecurityMode.Edit)]
        //[ContractMethod("RecalculateTaskInstanceAssignees")]
        //void RecalculateAssignees(RecalculateTaskInstancesAssigneesModel recalculateTaskInstancesModel);
    }
}
