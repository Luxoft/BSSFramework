using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Framework.Authorization.ApproveWorkflow;
using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DTO;
using Framework.Core.Services;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.DomainDriven.WebApiNetCore;
using Framework.Exceptions;
using Framework.SecuritySystem.Exceptions;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.BLL;
using SampleSystem.Domain;
using SampleSystem.Generated.DTO;
using SampleSystem.ServiceEnvironment;

using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace SampleSystem.WebApiCore.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class WorkflowController : ApiControllerBase<
        SampleSystemServiceEnvironment,
        ISampleSystemBLLContext, EvaluatedData<ISampleSystemBLLContext, ISampleSystemDTOMappingService>>
{
    private readonly StartWorkflowJob startWorkflowJob;

    private readonly IWorkflowHost workflowHost;

    private readonly IUserAuthenticationService userAuthenticationService;

    public WorkflowController(
            SampleSystemServiceEnvironment environment,
            IExceptionProcessor exceptionProcessor,
            StartWorkflowJob startWorkflowJob,
            IWorkflowHost workflowHost,
            IUserAuthenticationService userAuthenticationService)
            : base(environment, exceptionProcessor)
    {
        this.startWorkflowJob = startWorkflowJob;
        this.workflowHost = workflowHost;
        this.userAuthenticationService = userAuthenticationService;
    }

    protected override EvaluatedData<ISampleSystemBLLContext, ISampleSystemDTOMappingService> GetEvaluatedData(IDBSession session, ISampleSystemBLLContext context) =>
            new(session, context, new SampleSystemServerPrimitiveDTOMappingService(context));


    [HttpPost(nameof(StartJob))]
    public Dictionary<Guid, Guid> StartJob()
    {
        this.EvaluateC(DBSessionMode.Read, ctx => ctx.Authorization.CheckAccess(SampleSystemSecurityOperation.SystemIntegration));

        return this.startWorkflowJob.Start();
    }

    [HttpPost(nameof(GetMyPendingApproveOperationWorkflowObjects))]
    public async Task<List<ApproveOperationWorkflowObject>> GetMyPendingApproveOperationWorkflowObjects(PermissionIdentityDTO permissionIdent)
    {
        var workflowOperationIdents = this.Evaluate(DBSessionMode.Read, evaluateData =>
        {
            var permissionIdStr = permissionIdent.Id.ToString();

            var bll = evaluateData.Context.Logics.Default.Create<WorkflowCoreInstance>();

            var instances = bll.GetListBy(wi =>
                    wi.Data.Contains(permissionIdStr)
                    && wi.WorkflowDefinitionId == nameof(__ApproveOperation_Workflow)
                    && wi.Status == WorkflowStatus.Runnable);

            return instances.ToList().Select(wi => wi.Id);
        });

        var result = new List<ApproveOperationWorkflowObject>();

        foreach (var wiId in workflowOperationIdents)
        {
            var wi = await this.workflowHost.PersistenceStore.GetWorkflowInstance(wiId.ToString());

            var opWfObj = (ApproveOperationWorkflowObject)wi.Data;

            if (opWfObj.ApproveEventId != null && opWfObj.RejectEventId != null)
            {
                result.Add(opWfObj);
            }
        }

        this.Evaluate(DBSessionMode.Read, evaluateData =>
        {
            var authContext = evaluateData.Context.Authorization;

            result.RemoveAll(wfObj =>
            {
                var operation = authContext.Logics.Operation.GetById(wfObj.OperationId, true);

                return !authContext.GetOperationSecurityProvider().HasAccess(operation);
            });
        });

        return result;
    }

    [HttpPost(nameof(ApproveOperation))]
    public Task ApproveOperation(PermissionIdentityDTO permissionIdentity, string approveEventId)
    {
        return this.ApproveRejectOperation(permissionIdentity, approveEventId, true);
    }

    [HttpPost(nameof(RejectOperation))]
    public Task RejectOperation(PermissionIdentityDTO permissionIdentity, string rejectEventId)
    {
        return this.ApproveRejectOperation(permissionIdentity, rejectEventId, false);
    }

    public async Task ApproveRejectOperation(PermissionIdentityDTO permissionIdentity, string eventId, bool isApprove)
    {
        var permissionIdStr = permissionIdentity.Id.ToString();

        var wiId = this.EvaluateC(
            DBSessionMode.Read,
            ctx => ctx.Logics.WorkflowCoreInstance.GetObjectBy(ee => ee.Data.Contains(permissionIdStr) && ee.Data.Contains(eventId), true).Id);

        await this.workflowHost.PublishEvent(__ApproveOperation_Workflow.GetEventName(isApprove), eventId, this.userAuthenticationService.GetUserName());

        await Task.Delay(3000); // need refact

        this.EvaluateC(
            DBSessionMode.Read,
            ctx =>
            {
                var error = ctx.Logics.WorkflowCoreExecutionError.GetObjectBy(ee => ee.Message.Contains(permissionIdStr) && ee.Message.Contains(eventId) && ee.WorkflowInstance.Id == wiId);

                if (error != null)
                {
                    throw new AccessDeniedException<Guid>(nameof(Permission), permissionIdentity.Id, error.Message);
                }
            });
    }
}
