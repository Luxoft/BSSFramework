using System;
using System.Collections.Generic;

using Framework.Authorization.Domain;

namespace Framework.Authorization.ApproveWorkflow;

public class ApprovePermissionWorkflowObject
{
    public Guid PermissionId { get; set; }

    public string Name { get; set; }

    public PermissionStatus Status { get; set; }

    public List<ApproveOperationWorkflowObject> Operations { get; set; }

    public bool SomeOneOperationRejected { get; set; }
}
