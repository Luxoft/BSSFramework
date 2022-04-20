using System;

using Framework.DomainDriven.BLL;
using Framework.Persistent.Mapping;

namespace SampleSystem.Domain;

[BLLRole]
[View]
public class WorkflowCoreExecutionError : PersistentDomainObjectBase
{
    private string message;

    private DateTime errorTime;

    private WorkflowCoreInstance workflowInstance;

    public virtual string Message
    {
        get { return this.message; }
        set { this.message = value; }
    }

    public virtual DateTime ErrorTime
    {
        get { return this.errorTime; }
        set { this.errorTime = value; }
    }

    public virtual WorkflowCoreInstance WorkflowInstance
    {
        get { return this.workflowInstance; }
        set { this.workflowInstance = value; }
    }
}
