using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;

using JetBrains.Annotations;

namespace Framework.Authorization.ApproveWorkflow;

public class WorkflowApproveProcessor : BLLContextContainer<IAuthorizationBLLContext>, IWorkflowApproveProcessor
{
    public WorkflowApproveProcessor([NotNull] IAuthorizationBLLContext context)
            : base(context)
    {
    }

    public virtual ApprovePermissionWorkflowObject GetPermissionStartupObject(Permission permission)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        return new ApprovePermissionWorkflowObject
               {
                       PermissionId = permission.Id,
                       Name = $"Approving \"{permission.Role.Name}\" Permission Role \"{permission.Principal.Name}\" with duration \"{permission.Period}\"",
                       Operations = this.GetApproveOperationStartupObjects(permission).ToList()
               };
    }

    private IEnumerable<ApproveOperationWorkflowObject> GetApproveOperationStartupObjects(Permission permission)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        return from link in permission.Role.BusinessRoleOperationLinks

               let operation = link.Operation

               where operation.ApproveOperation != null

               group operation by operation.ApproveOperation into g

               let approveOperation = g.Key

               select new ApproveOperationWorkflowObject
                      {
                              OperationId = approveOperation.Id,
                              Name = $"Approve by operation: {approveOperation.Name}",
                              PermissionId = permission.Id,
                              Description = $"Approving operations: {g.OrderBy(op => op.Name).Join(", ", op => op.Name)}"
                      };
    }

    public virtual bool CanAutoApprove(Permission permission, Operation approveOperation)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));
        if (approveOperation == null) throw new ArgumentNullException(nameof(approveOperation));

        var createdByPrincipal = permission.CreatedBy == this.Context.CurrentPrincipalName
                                         ? this.Context.CurrentPrincipal
                                         : this.Context.Logics.Principal.GetByName(permission.CreatedBy);

        var autoApprove = permission.DelegatedFrom != null && createdByPrincipal.GetOperations(this.Context.DateTimeService.Now).Contains(approveOperation);

        return autoApprove;
    }
}
