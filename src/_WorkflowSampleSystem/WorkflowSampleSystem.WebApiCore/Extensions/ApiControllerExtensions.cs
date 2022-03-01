using System;

using Framework.CustomReports.Domain;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.DomainDriven.WebApiNetCore;

using WorkflowSampleSystem.BLL;
using WorkflowSampleSystem.Generated.DTO;

using Microsoft.AspNetCore.Mvc;

namespace WorkflowSampleSystem.WebApiCore.Controllers.CustomReport
{
    public static class ApiControllerExtensions
    {
        public static FileStreamResult GetReportResult(
                this ApiControllerBase<IServiceEnvironment<IWorkflowSampleSystemBLLContext>, IWorkflowSampleSystemBLLContext, EvaluatedData<IWorkflowSampleSystemBLLContext, IWorkflowSampleSystemDTOMappingService>>
                        controller,
                IReportStream result)
        {
            return Framework.CustomReports.WebApi.ApiControllerExtensions.GetReportResult(controller, result);
        }
    }
}
