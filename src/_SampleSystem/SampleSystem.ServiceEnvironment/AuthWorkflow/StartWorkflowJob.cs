using Framework.Authorization.ApproveWorkflow;
using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven;

using SampleSystem.BLL;

using WorkflowCore.Interface;

namespace SampleSystem.ServiceEnvironment;

public class StartWorkflowJob
{
    private readonly IServiceEvaluator<ISampleSystemBLLContext> contextEvaluator;

    private readonly IWorkflowHost workflowHost;

    private readonly IWorkflowApproveProcessor workflowApproveProcessor;


    public StartWorkflowJob(IServiceEvaluator<ISampleSystemBLLContext> contextEvaluator, IWorkflowHost workflowHost, IWorkflowApproveProcessor workflowApproveProcessor)
    {
        this.contextEvaluator = contextEvaluator;
        this.workflowHost = workflowHost;
        this.workflowApproveProcessor = workflowApproveProcessor;
    }

    public async Task<Dictionary<Guid, Guid>> Start()
    {
        return await this.contextEvaluator.EvaluateAsync(DBSessionMode.Write, async ctx =>
        {
            var permQ = ctx.Authorization.Logics.Permission.GetUnsecureQueryable();

            var wfObjRequest =

                    from wfObj in ctx.Logics.ApprovePermissionWorkflowDomainObject.GetUnsecureQueryable()

                    where wfObj.WorkflowInstanceId == null

                    join permission in permQ on wfObj.PermissionId equals permission.Id

                    where permission.Status == PermissionStatus.Approving

                    select new { wfObj, permission };

            var wfObjList = wfObjRequest.ToList();


            return await wfObjList.ToDictionaryAsync(pair => pair.wfObj.PermissionId,
                async pair =>
                {
                    var startupObj = this.workflowApproveProcessor.GetPermissionStartupObject(pair.permission);

                    var wfInstanceIdStr = await this.workflowHost.StartWorkflow(nameof(__ApprovePermission_Workflow), startupObj);
                    var wfInstanceId = new Guid(wfInstanceIdStr);
                    pair.wfObj.WorkflowInstanceId = wfInstanceId;

                    ctx.Logics.ApprovePermissionWorkflowDomainObject.Save(pair.wfObj);

                    return wfInstanceId;
                });
        });
    }
}
