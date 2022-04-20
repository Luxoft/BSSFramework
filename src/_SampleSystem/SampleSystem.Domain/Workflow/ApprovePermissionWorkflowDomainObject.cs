using System;

using Framework.DomainDriven.BLL;

namespace SampleSystem.Domain;

[BLLRole]
public class ApprovePermissionWorkflowDomainObject : AuditPersistentDomainObjectBase
{
    private Guid permissionId;

    private Guid? workflowInstanceId;

    public virtual Guid PermissionId
    {
        get { return this.permissionId; }
        set { this.permissionId = value; }
    }

    public virtual Guid? WorkflowInstanceId
    {
        get { return this.workflowInstanceId; }
        set { this.workflowInstanceId = value; }
    }
}
