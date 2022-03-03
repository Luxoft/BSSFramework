using System;

using Framework.Workflow.BLL;
using Framework.Workflow.Generated.DTO;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.DomainDriven.WebApiNetCore;
using Framework.Exceptions;
using Framework.WebApi.Utils.SL;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;

namespace Framework.Workflow.WebApi
{
    [SLJsonCompatibilityActionFilter]
    [TypeFilter(typeof(SLJsonCompatibilityResourceFilter))]
    [ApiController]
    [Route("WorkflowSLJsonFacade.svc")]
    [ApiExplorerSettings(IgnoreApi = true)]
    //[Authorize(nameof(AuthenticationSchemes.NTLM))]
    public abstract partial class WorkflowSLJsonController : ApiControllerBase<IWorkflowServiceEnvironment, IWorkflowBLLContext, EvaluatedData<IWorkflowBLLContext, IWorkflowDTOMappingService>>
    {
        protected WorkflowSLJsonController(IWorkflowServiceEnvironment environment, IExceptionProcessor exceptionProcessor)
            : base(environment, exceptionProcessor)
        {

        }

        protected override EvaluatedData<IWorkflowBLLContext, IWorkflowDTOMappingService> GetEvaluatedData(IDBSession session, IWorkflowBLLContext context)
        {
            return new EvaluatedData<IWorkflowBLLContext, IWorkflowDTOMappingService>(session, context, new WorkflowServerPrimitiveDTOMappingService(context));
        }
    }
}
