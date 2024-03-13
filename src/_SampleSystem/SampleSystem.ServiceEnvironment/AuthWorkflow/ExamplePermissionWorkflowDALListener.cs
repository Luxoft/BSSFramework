using Framework.Authorization.Domain;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.BLL;
using SampleSystem.Domain;

namespace SampleSystem.ServiceEnvironment;

public class ExamplePermissionWorkflowDALListener : BLLContextContainer<ISampleSystemBLLContext>, IBeforeTransactionCompletedDALListener
{
    public ExamplePermissionWorkflowDALListener(ISampleSystemBLLContext context)
        : base(context)
    {
    }

    public void Process(DALChangesEventArgs eventArgs)
    {
        var permissionChanges = eventArgs.Changes.GetSubset(typeof(Permission));

        if (!this.Context.ServiceProvider.GetRequiredService<IWorkflowManager>().Enabled)
        {
            return;
        }

        foreach (Permission permission in permissionChanges.CreatedItems.Select(dalObj => dalObj.Object))
        {
            permission.Status = PermissionStatus.Approving;

            this.Context.Authorization.Logics.Permission.Save(permission);

            this.Context.Logics.ApprovePermissionWorkflowDomainObject.Save(new ApprovePermissionWorkflowDomainObject
            {
                    PermissionId = permission.Id
            });
        }

        //foreach (Permission permission in permissionChanges.RemovedItems.Select(dalObj => dalObj.Object))
        //{
        //    var startupObj = this.workflowApproveProcessor.GetPermissionStartupObject(permission);

        //    var ident = await this.workflowHost.StartWorkflow(nameof(__ApprovePermission_Workflow), startupObj);
        //}
    }
}
