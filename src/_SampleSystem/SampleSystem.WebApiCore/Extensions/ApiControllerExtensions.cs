using System;

using Framework.CustomReports.Domain;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.DomainDriven.WebApiNetCore;

using SampleSystem.BLL;
using SampleSystem.Generated.DTO;

using Microsoft.AspNetCore.Mvc;

namespace SampleSystem.WebApiCore.Controllers.CustomReport
{
    public static class ApiControllerExtensions
    {
        public static FileStreamResult GetReportResult(
                this ApiControllerBase<IServiceEnvironment<ISampleSystemBLLContext>, ISampleSystemBLLContext, EvaluatedData<ISampleSystemBLLContext, ISampleSystemDTOMappingService>>
                        controller,
                IReportStream result)
        {
            return Framework.CustomReports.WebApi.ApiControllerExtensions.GetReportResult(controller, result);
        }
    }
}
