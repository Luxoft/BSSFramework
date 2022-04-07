using Framework.DomainDriven.BLL;
using Framework.Persistent.Mapping;

using WorkflowCore.Models;

namespace SampleSystem.Domain;

[BLLRole]
[View]
public class WorkflowCoreInstance : PersistentDomainObjectBase
{
    private WorkflowStatus status;

    private string data;

    private string workflowDefinitionId;

    public virtual string Data
    {
        get { return this.data; }
        set { this.data = value; }
    }

    public virtual string WorkflowDefinitionId
    {
        get { return this.workflowDefinitionId; }
        set { this.workflowDefinitionId = value; }
    }

    public WorkflowStatus Status
    {
        get { return this.status; }
        set { this.status = value; }
    }
}
