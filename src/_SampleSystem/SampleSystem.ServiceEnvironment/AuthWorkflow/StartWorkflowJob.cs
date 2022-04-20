using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Framework.Authorization.ApproveWorkflow;
using Framework.Authorization.Domain;
using Framework.DomainDriven.BLL;

using SampleSystem.BLL;

using WorkflowCore.Interface;

namespace SampleSystem.ServiceEnvironment;

public class StartWorkflowJob
{
    private readonly IContextEvaluator<ISampleSystemBLLContext> contextEvaluator;

    private readonly IWorkflowHost workflowHost;

    private readonly IWorkflowApproveProcessor workflowApproveProcessor;


    public StartWorkflowJob(IContextEvaluator<ISampleSystemBLLContext> contextEvaluator, IWorkflowHost workflowHost, IWorkflowApproveProcessor workflowApproveProcessor)
    {
        this.contextEvaluator = contextEvaluator;
        this.workflowHost = workflowHost;
        this.workflowApproveProcessor = workflowApproveProcessor;
    }

    public Dictionary<Guid, Guid> Start()
    {
        return this.contextEvaluator.Evaluate(DBSessionMode.Write, ctx =>
        {
            var permQ = ctx.Authorization.Logics.Permission.GetUnsecureQueryable();

            var wfObjRequest =

                    from wfObj in ctx.Logics.ApprovePermissionWorkflowDomainObject.GetUnsecureQueryable()

                    where wfObj.WorkflowInstanceId == null

                    join permission in permQ on wfObj.PermissionId equals permission.Id

                    where permission.Status == PermissionStatus.Approving

                    select new { wfObj, permission };

            var wfObjList = wfObjRequest.ToList();


            return wfObjList.ToDictionary(pair => pair.wfObj.PermissionId,
                pair =>
                {
                    var startupObj = this.workflowApproveProcessor.GetPermissionStartupObject(pair.permission);

                    var wfInstanceId = new Guid(Task.Run(() => this.workflowHost.StartWorkflow(nameof(__ApprovePermission_Workflow), startupObj)).Result);
                    pair.wfObj.WorkflowInstanceId = wfInstanceId;

                    ctx.Logics.ApprovePermissionWorkflowDomainObject.Save(pair.wfObj);

                    return wfInstanceId;
                });
        });
    }
}
