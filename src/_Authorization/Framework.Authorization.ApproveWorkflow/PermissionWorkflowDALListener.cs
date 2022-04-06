using System;
using System.Linq;

using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.DomainDriven.BLL;

using JetBrains.Annotations;

using WorkflowCore.Interface;

namespace Framework.Authorization.ApproveWorkflow;

public class PermissionWorkflowDALListener : IDALListener
{
    private readonly IAuthorizationBLLContext bllContext;

    private readonly IWorkflowHost workflowHost;

    private readonly IWorkflowApproveProcessor workflowApproveProcessor;

    public PermissionWorkflowDALListener([NotNull] IAuthorizationBLLContext bllContext, IWorkflowHost workflowHost, IWorkflowApproveProcessor workflowApproveProcessor)
    {
        this.bllContext = bllContext ?? throw new ArgumentNullException(nameof(bllContext));
        this.workflowHost = workflowHost;
        this.workflowApproveProcessor = workflowApproveProcessor;
    }

    public async void Process(DALChangesEventArgs eventArgs)
    {
        var permissionChanges = eventArgs.Changes.GetSubset(typeof(Permission));

        foreach (Permission permission in permissionChanges.CreatedItems.Select(dalObj => dalObj.Object))
        {
            var startupObj = this.workflowApproveProcessor.GetPermissionStartupObject(permission);

            var ident = await this.workflowHost.StartWorkflow(nameof(__ApprovePermission_Workflow), startupObj);
        }

        //foreach (Permission permission in permissionChanges.RemovedItems.Select(dalObj => dalObj.Object))
        //{
        //    var startupObj = this.workflowApproveProcessor.GetPermissionStartupObject(permission);

        //    var ident = await this.workflowHost.StartWorkflow(nameof(__ApprovePermission_Workflow), startupObj);
        //}
    }
}
