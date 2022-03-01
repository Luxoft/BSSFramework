using System;

using Framework.CustomReports.WebApi;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.Exceptions;

using Microsoft.AspNetCore.Mvc;

using WorkflowSampleSystem.BLL;
using WorkflowSampleSystem.Domain;
using WorkflowSampleSystem.Generated.DTO;

using WorkflowSampleSystem.WebApiCore.CustomReports;

namespace WorkflowSampleSystem.WebApiCore.Controllers.Report
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("reportApi/v{version:apiVersion}/[controller]")]
    public class WorkflowSampleSystemGenericReportController : GenericReportControllerBase<WorkflowSampleSystemCustomReportsServiceEnvironment, IWorkflowSampleSystemBLLContext,
        PersistentDomainObjectBase,
        WorkflowSampleSystemSecurityOperationCode, Guid, IWorkflowSampleSystemDTOMappingService>
    {
        public WorkflowSampleSystemGenericReportController(WorkflowSampleSystemCustomReportsServiceEnvironment environment, IExceptionProcessor exceptionProcessor)
            : base(environment, exceptionProcessor)
        {
        }

        protected override EvaluatedData<IWorkflowSampleSystemBLLContext, IWorkflowSampleSystemDTOMappingService> GetEvaluatedData(IDBSession session, IWorkflowSampleSystemBLLContext context)
        {
            return new EvaluatedData<IWorkflowSampleSystemBLLContext, IWorkflowSampleSystemDTOMappingService>(
                session,
                context,
                new WorkflowSampleSystemServerPrimitiveDTOMappingService(context));
        }
    }
}
