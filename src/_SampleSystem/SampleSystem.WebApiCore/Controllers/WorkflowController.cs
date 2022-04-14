using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Framework.Authorization.ApproveWorkflow;
using Framework.Authorization.BLL;
using Framework.Authorization.Generated.DTO;
using Framework.Core.Services;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.DomainDriven.WebApiNetCore;
using Framework.Exceptions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

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
        IServiceEnvironment<ISampleSystemBLLContext>,
        ISampleSystemBLLContext, EvaluatedData<ISampleSystemBLLContext, ISampleSystemDTOMappingService>>
{
    private readonly StartWorkflowJob startWorkflowJob;

    private readonly IWorkflowHost workflowHost;

    private readonly IUserAuthenticationService userAuthenticationService;

    public WorkflowController(
            IServiceEnvironment<ISampleSystemBLLContext> environment,
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
        return this.startWorkflowJob.Start();
    }

    [HttpPost(nameof(GetMyApproveOperationWorkflowObjects))]
    public async Task<List<ApproveOperationWorkflowObject>> GetMyApproveOperationWorkflowObjects(PermissionIdentityDTO permissionIdent)
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
    public async Task ApproveOperation(string approveEventId)
    {
        await this.workflowHost.PublishEvent(__ApproveOperation_Workflow.GetEventName(true), approveEventId, this.userAuthenticationService.GetUserName());
    }

    [HttpPost(nameof(RejectOperation))]
    public async Task RejectOperation(string rejectEventId)
    {
        await this.workflowHost.PublishEvent(__ApproveOperation_Workflow.GetEventName(false), rejectEventId, this.userAuthenticationService.GetUserName());
    }
}
